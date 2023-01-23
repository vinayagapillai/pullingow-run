using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public GameObject _playerPrefab;

    public static GameManager Instance;

    [HideInInspector]
    public GameObject spawnedPlayer;

    private Vector3 _playerIntialTransform;
    private bool _isGameOver = false;

    public int Score { get; private set; }

    [SerializeField]
    private GameObject _spawnPoint;
    [SerializeField]
    private UnityEvent<bool> _gameOverEvent;

    private void Awake()
    {
        _playerIntialTransform = _playerPrefab.GetComponent<Transform>().transform.position;
        _playerPrefab.SetActive(true);
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        //SpawnPlayer();
    }

    //public void UpdateScore(int value)
    //{
    //    Score += value;
    //    UIManager.Instance.UpdateScoreUI(Score);
    //}

    //private void SpawnPlayer()
    //{
    //    spawnedPlayer = (GameObject)Instantiate(_playerPrefab, _spawnPoint.transform.position, _spawnPoint.transform.rotation);
    //    spawnedPlayer.transform.name = "Player";

    //    CameraManager.instance._virtualCamera.LookAt = spawnedPlayer.transform;
    //    CameraManager.instance._virtualCamera.Follow = spawnedPlayer.transform;
    //}

    public void GameOver()
    {
        _isGameOver = true;
        Debug.Log("GameOver");
        _gameOverEvent.Invoke(_isGameOver);
        _playerPrefab.SetActive(false);
    }

    public void RestartLevel()
    {
        _isGameOver = false;
        Debug.Log("I am pressed");

        //If player prefab is not instantiated
        _playerPrefab.transform.position = _playerIntialTransform;
        _playerPrefab?.SetActive(true);

        _gameOverEvent.Invoke(_isGameOver);
        //SpawnPlayer();
    }
}
