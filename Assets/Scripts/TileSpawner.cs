using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TempleRun
{
    public class TileSpawner : MonoBehaviour
    {
        [SerializeField]
        private int _tileStartCount = 10;
        [SerializeField]
        private int _maximumStraightTile = 10;

        [SerializeField]
        private GameObject _startingTile;    
        //[SerializeField]
        //private List<GameObject> _turnTiles;
        [SerializeField]
        private List<GameObject> _obstacles;        
        [SerializeField]
        private List<GameObject> _tiles;
        [SerializeField]
        private List<GameObject> _money;
        public Collider[] colliders;
        public float radius;

        private Vector3 _currentTileLocation = Vector3.zero;
        private Vector3 _currentTileDirection = Vector3.forward;
        private Vector3 _obstacleLocation;
        private Vector3 _moneyLocation;
        private Vector3 _tileLocation;
        private Vector3 _newMoneyLocation;
        private GameObject _prevTile;
        private GameObject _obstaclePrefab;
        private GameObject _moneyPrefab;

        private List<GameObject> _currentTiles;
        private List<GameObject> _currentObstacles;

        private int _spawnTilesCallcount = 0;
        private int _tileCountStorer = 0;
        private int _obstacleCountStorer = 0;

        private void OnEnable()
        {
            PlayerController.DestroyPreviousTiles += DeletePreviousTiles;
            PlayerController.DestroyPreviousTiles += DeletePreviousObstacles;
        }

        private void OnDisable()
        {
            PlayerController.DestroyPreviousTiles -= DeletePreviousTiles;
            PlayerController.DestroyPreviousTiles -= DeletePreviousObstacles;
        }

        private void Start()
        {
            AddGameObjectsToList(_startingTile);
            //_rightTile = _startingTile.transform.GetChild(0).gameObject;
            //_leftTile = _startingTile.transform.GetChild(1).gameObject;
            _currentTiles = new List<GameObject>();
            _currentObstacles = new List<GameObject>();

            //Generate radom seed for not repeating same values over and over again
            Random.InitState(System.DateTime.Now.Millisecond);

            SpawnTiles();
        }

        //Spawns a tile at the location rotated towards the direction currently we are moving at
        private void SpawnTiles(bool spawnObstacle = false, bool spawnMoney = true)
        {

            //Spawning straight tiles
            for (int i = 0; i < _tileStartCount; i++)
            {
                _spawnTilesCallcount++;
                //Rotate tile 90 degrees to match the current tile direction
                Quaternion newTileRotation = _startingTile.transform.rotation * Quaternion.LookRotation(_currentTileDirection, Vector3.up);

                _prevTile =  (GameObject)Instantiate(_startingTile, _currentTileLocation, newTileRotation);
                _currentTiles.Add(_prevTile);
                //if(count == 10)
                //    _currentTiles[i].name = _startingTile.name + (count).ToString();
                _currentTiles[i].name = _startingTile.name + (_spawnTilesCallcount).ToString();
                if (spawnObstacle) SpawnObastacle();
                if (spawnMoney) SpawnMoney();

                //Get the previous tile location and multiply to the cureent direction
                //Calculate the currentTileLocation omly if we are going straight
                _currentTileLocation += Vector3.Scale(_prevTile.GetComponent<Renderer>().bounds.size, _currentTileDirection);
                _tileCountStorer = _currentTiles.Count;
            }
            StartCoroutine(SpawnTilesLater());
        }

        IEnumerator SpawnTilesLater()
        {
            yield return new WaitForSeconds(3f);
            //DeletePreviousTiles();
            _spawnTilesCallcount++;
            SpawnTiles(true);
        }

        //Delete tiles and obstacles
        private void DeletePreviousTiles()
        {
            while(_currentTiles.Count != _tileCountStorer -10)
            {
                GameObject tile = _currentTiles[0];
                _currentTiles.RemoveAt(0);
                Destroy(tile);

            }
        }

        private void DeletePreviousObstacles()
        {
            while (_currentObstacles.Count != _obstacleCountStorer - 2)
            {
                GameObject obstacle = _currentObstacles[0];
                _currentObstacles.RemoveAt(0);
                Destroy(obstacle);
            }
        }

        //It finds the next tile spawn location once the player turns
        //public void AddNewDirection(Vector3 direction)
        //{
        //    _currentTileDirection = direction;
        //    

        //    Vector3 tilePlacementScale;
        //    if(_prevTile.GetComponent<Tile>().type == TileType.SIDEWAYS)
        //    {
        //        /*Spawn sideways tile */

        //        //Get the total size of the previous tile and divid by 2
        //        //get the half size of the next tile to be spawned and divide by 2
        //        //then finally mutiply with cur
        //        tilePlacementScale = Vector3.Scale(_prevTile.GetComponent<Renderer>().bounds.size / 2  + (Vector3.one * _startingTile.GetComponent<BoxCollider>().size.z / 2), _currentTileDirection);
        //    }
        //    else
        //    {
        //        /*Spawn Left or Right side tile */

        //        //since there are 12 extra tiles, subract 2 from the present left or right tile
        //        tilePlacementScale = Vector3.Scale((_prevTile.GetComponent<Renderer>().bounds.size - (Vector3.one * 2)) + (Vector3.one * _startingTile.GetComponent<BoxCollider>().size.z / 2), _currentTileDirection);
        //        Debug.Log("Tile Placement Scale:" + tilePlacementScale);
        //    }

        //    //Add the found tileplacement to the current tile location
        //    _currentTileLocation += tilePlacementScale;

        //    //Spawn certain amount of straight tiles after turning
        //    int currentPathLength = Random.Range(_minimumStraightTile, _maximumStraightTile);
        //    for (int i = 0; i < currentPathLength; i++)
        //    {
        //        //Spawn tile
        //        //spawn obstacle but not when the next tile spawned
        //        SpawnTiles(_startingTile.GetComponent<Tile>(), (i == 0) ? false : true);
        //    }

        //    //Spawn a turn tile
        //    SpawnTiles(SelectRandomGameObjectFromList(_turnTiles).GetComponent<Tile>(), false);
        //}

        //Spawn obstacle
        private void SpawnObastacle()
        {
            //We have 20% chance to spawn and obstacle and 80% not
            if (Random.value > 0.4f) return;

            _obstaclePrefab = SelectRandomGameObjectFromList(_obstacles);
            GameObject tilePrefab = SelectRandomGameObjectFromList(_tiles);

            _obstacleLocation = new Vector3(tilePrefab.transform.position.x, 0f, 0f);
            Quaternion newObjectRotation;

            //Chcking and turning the direction of the dog 
            if (_obstaclePrefab.transform.name == "Dog_Obstacle" && tilePrefab.transform.position.x == 4)
            {
                newObjectRotation = _obstaclePrefab.gameObject.transform.rotation * Quaternion.LookRotation(_currentTileDirection, Vector3.up) * Quaternion.Euler(0, 180, 0);
                
            }
            else
            {

            //Rotate object 90 degrees to match the current tile direction
            newObjectRotation = _obstaclePrefab.gameObject.transform.rotation * Quaternion.LookRotation(_currentTileDirection, Vector3.up);
            }

            GameObject obstacle = (GameObject)Instantiate(_obstaclePrefab, _currentTileLocation + _obstacleLocation, newObjectRotation);
            _currentObstacles.Add(obstacle);
            _obstacleCountStorer = _currentObstacles.Count;
        }

        private void SpawnMoney()
        {
            // We have 20 % chance to spawn and obstacle and 80 % not
            if (Random.value > 0.9f) return;

            _moneyPrefab = SelectRandomGameObjectFromList(_money);
            GameObject tilePrefab = SelectRandomGameObjectFromList(_tiles);

            _moneyLocation = new Vector3(tilePrefab.transform.position.x, 0f, 0f);
            Quaternion newObjectRotation = _moneyPrefab.gameObject.transform.rotation * Quaternion.LookRotation(_currentTileDirection, Vector3.up);
            GameObject money = (GameObject)Instantiate(_moneyPrefab, _currentTileLocation + _moneyLocation + _newMoneyLocation, newObjectRotation);
            _newMoneyLocation = new Vector3(0f, 0f, money.transform.position.z + 0.5f); 
            //colliders = Physics.OverlapSphere(money.transform.position, radius);
            //foreach (Collider col in colliders)
            //{
            //    if (col.transform.name == "Dog_Obstacle(Clone)")
            //    {
            //        Debug.Log("I am inside dog");
            //    }
            //}
        }

        //Select a random Gameobject
        private GameObject SelectRandomGameObjectFromList(List<GameObject> list)
        {
            if (list.Count == 0)
                return null;
            return list[Random.Range(0, list.Count)];
        }

        private void AddGameObjectsToList(GameObject gObject)
        {
            if(gObject != null)
            {
                _tiles.Add(gObject);
                for (int i = 0; i < gObject.transform.childCount; i++)
                {
                    _tiles.Add(gObject.transform.GetChild(i).gameObject);
                }
                
            }
        }
    }
}