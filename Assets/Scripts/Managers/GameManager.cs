using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject _playerPrefab;

    public static GameManager Instance;

    [HideInInspector]
    public GameObject spawnedPlayer;

    private bool _isGameOver = false;
    private int _activeScene;

    public int Score { get; private set; }

    [SerializeField]
    private GameObject _spawnPoint;

    private void Awake()
    {
         _activeScene = SceneManager.GetActiveScene().buildIndex;
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
        UIManager.Instance.ShowGameoverUI(_isGameOver);
        _playerPrefab.SetActive(!_isGameOver);
    }

    public void RestartLevel()
    {
        _isGameOver = false;
        //If player prefab is not instantiated
        SceneManager.LoadScene(_activeScene);
        UIManager.Instance.ShowGameoverUI(_isGameOver);
        _playerPrefab.SetActive(!_isGameOver);
        //SpawnPlayer();
    }
}
