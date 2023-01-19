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
    }
}