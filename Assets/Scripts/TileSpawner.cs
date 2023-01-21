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
        private int _minimumStraightTile = 3; 
        [SerializeField]
        private int _maximumStraightTile = 10;

        [SerializeField]
        private GameObject _startingTile;
        [SerializeField]
        private List<GameObject> _turnTiles;
        [SerializeField]
        private List<GameObject> _obstacles;

        private Vector3 _currentTileLocation = Vector3.zero;
        private Vector3 _currentTileDirection = Vector3.forward;
        private GameObject _prevTile;

        private List<GameObject> _currentTiles;
        private List<GameObject> _currentObstacles;

        private void Start()
        {
            _currentTiles = new List<GameObject>();
            _currentObstacles = new List<GameObject>();

            Random.InitState(System.DateTime.Now.Millisecond);

            for (int i = 0; i < _tileStartCount; i++)
            {
                //Spawn tiles and check if obstacle needed
                SpawnTiles(_startingTile.GetComponent<Tile>(), false);
            }

            //SpawnTiles();
        }

        private void SpawnTiles(Tile tile, bool spawnObstacle)
        {
            _prevTile =  GameObject.Instantiate(tile.gameObject, _currentTileLocation, Quaternion.identity);
            _currentTiles.Add(_prevTile);

            //Get the previous tile location and multiply to the cureent direction
            _currentTileLocation += Vector3.Scale(_prevTile.GetComponent<Renderer>().bounds.size, _currentTileDirection);
        }
    }
}