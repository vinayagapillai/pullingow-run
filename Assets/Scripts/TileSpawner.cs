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

        private Vector3 _currentTileLocation = Vector3.zero;
        private Vector3 _currentTileDirection = Vector3.forward;
        private GameObject _prevTile;

        private List<GameObject> _currentTiles;
        private List<GameObject> _currentObstacles;

        private bool _isMaxTileReached = false;

        private void Start()
        {
            _currentTiles = new List<GameObject>();
            _currentObstacles = new List<GameObject>();

            //Generate radom seed for not repeating same values over and over again
            Random.InitState(System.DateTime.Now.Millisecond);

            SpawnTiles();
        }

        //Spawns a tile at the location rotated towards the direction currently we are moving at
        private void SpawnTiles(bool spawnObstacle = false)
        {
            //Spawning straight tiles
            for (int i = 0; i < _tileStartCount; i++)
            {

                //Rotate tile 90 degrees to match the current tile direction
                Quaternion newTileRotation = _startingTile.transform.rotation * Quaternion.LookRotation(_currentTileDirection, Vector3.up);

                _prevTile =  (GameObject)Instantiate(_startingTile, _currentTileLocation, newTileRotation);
                _currentTiles.Add(_prevTile);
                _currentTiles[i].name = _startingTile.name + i.ToString();
                if (spawnObstacle) SpawnObastacle();

                //Get the previous tile location and multiply to the cureent direction
                //Calculate the currentTileLocation omly if we are going straight
                _currentTileLocation += Vector3.Scale(_prevTile.GetComponent<Renderer>().bounds.size, _currentTileDirection);
            }
            StartCoroutine(SpawnTilesLater());
        }

        IEnumerator SpawnTilesLater()
        {
            yield return new WaitForSeconds(6f);
            //DeletePreviousTiles();
            SpawnTiles(true);
        }

        //Delete tiles and obstacles
        private void DeletePreviousTiles()
        {
            Debug.Log("Current Tiles count:" + _currentTiles.Count);
            while(_currentTiles.Count != 7)
            {
                GameObject tile = _currentTiles[0];
                _currentTiles.RemoveAt(0);
                Destroy(tile);

            }

            while (_currentObstacles.Count != 0)
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

            GameObject obstaclePrefab = SelectRandomGameObjectFromList(_obstacles);

            //Rotate object 90 degrees to match the current tile direction
            Quaternion newObjectRotation = obstaclePrefab.gameObject.transform.rotation * Quaternion.LookRotation(_currentTileDirection, Vector3.up);

            GameObject obstacle = (GameObject)Instantiate(obstaclePrefab, _currentTileLocation, newObjectRotation);
            _currentObstacles.Add(obstacle);
        }

        //Select a random Gameobject
        private GameObject SelectRandomGameObjectFromList(List<GameObject> list)
        {
            if (list.Count == 0)
                return null;
            return list[Random.Range(0, list.Count)];
        }
    }
}