using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TempleRun
{

    public class BuildingSpawner : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> _buildingList = new List<GameObject>();
        [SerializeField]
        private GameObject _startingBuilding;

        private List<GameObject> _currentLeftBuildings;
        private List<GameObject> _currentRightBuildings;
        private int _startingBuildingCount = 30;
        private int _spawnBuildingSpawnCount;
        private Vector3 _currentLeftBuildingLocation = new Vector3(-9.15f, 0, 0);
        private Vector3 _currentRightBuildingLocation = new Vector3(9.15f, 0, 0);
        private Vector3 _currentBuildingDirection = Vector3.forward;
        private GameObject _prevLeftBuilding;
        private GameObject _prevRightBuilding;
        private int _currentLeftBuildingCountStorer = 0;
        private int _currentRightBuildingCountStorer = 0;

        private void OnEnable()
        {
            PlayerController.DestroyPreviousBuildings += DeletePreviousBuildings;
        }

        private void OnDisable()
        {
            PlayerController.DestroyPreviousBuildings -= DeletePreviousBuildings;
        }

        void Start()
        {
            _currentLeftBuildings = new List<GameObject>();
            _currentRightBuildings = new List<GameObject>();

            Random.InitState(System.DateTime.Now.Millisecond);

            SpawnBuildings();
        }

        void SpawnBuildings()
        {
            _startingBuildingCount++;
            for (int i = 0; i < _startingBuildingCount; i++)
            {
                Quaternion newLeftBuildingRotation = _startingBuilding.transform.rotation * Quaternion.LookRotation(_currentBuildingDirection, Vector3.up);

                _prevLeftBuilding = (GameObject)Instantiate(SelectRandomGameObjectFromList(_buildingList), _currentLeftBuildingLocation, newLeftBuildingRotation);
                _currentLeftBuildings.Add(_prevLeftBuilding);
                _currentLeftBuildings[i].name = _startingBuilding.name + (_currentLeftBuildings).ToString();
                _currentLeftBuildingLocation += Vector3.Scale(_prevLeftBuilding.GetComponent<Renderer>().bounds.size*1.5f, _currentBuildingDirection);

                Quaternion newRightBuildingRotation = (_startingBuilding.transform.rotation) * Quaternion.LookRotation(_currentBuildingDirection, Vector3.up) * Quaternion.Euler(0, 0, 180);
                _prevRightBuilding = (GameObject)Instantiate(SelectRandomGameObjectFromList(_buildingList), _currentRightBuildingLocation, newRightBuildingRotation);
                _currentRightBuildings.Add(_prevRightBuilding);
                _currentRightBuildings[i].name = _startingBuilding.name + (_currentRightBuildings).ToString();
                _currentRightBuildingLocation += Vector3.Scale(_prevRightBuilding.GetComponent<Renderer>().bounds.size*1.5f, _currentBuildingDirection);

                _currentLeftBuildingCountStorer = _currentLeftBuildings.Count;
                _currentRightBuildingCountStorer = _currentLeftBuildings.Count;
            }
            StartCoroutine(SpawnBuildingsLater());
        }

        IEnumerator SpawnBuildingsLater()
        {
            yield return new WaitForSeconds(6f);
            //DeletePreviousTiles();
            _startingBuildingCount++;
            SpawnBuildings();
        }

        private GameObject SelectRandomGameObjectFromList(List<GameObject> list)
        {
            if (list.Count == 0)
                return null;
            return list[Random.Range(0, list.Count)];
        }

        private void DeletePreviousBuildings()
        {
            while ((_currentLeftBuildings.Count != _currentLeftBuildingCountStorer - 15) && (_currentRightBuildings.Count != _currentRightBuildingCountStorer - 15))
            {
                GameObject buildingLeft = _currentLeftBuildings[0];
                GameObject buildingRight = _currentRightBuildings[0];
                _currentLeftBuildings.RemoveAt(0);
                _currentRightBuildings.RemoveAt(0);
                Destroy(buildingLeft);
                Destroy(buildingRight);

            }
        }
    }
}
