using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;


public class GameManager : MonoBehaviour
{
    public GameObject _playerPrefab;

    public static GameManager Instance;
    public static event Action<GameState> OnGameStateChanged;

    [HideInInspector]
    public GameObject spawnedPlayer;
    private int _activeScene;

    private bool _isGameOver = false;

    public int Score { get; private set; }

    [SerializeField]
    private GameObject _spawnPoint;
    [SerializeField]
    private GameState _gameStates;
    [SerializeField]
    private GameObject _pauseScreen; 
    [SerializeField]
    private GameObject _mainMenu;
    [SerializeField]
    private GameObject _gamePlayScreen;


    private void Awake()
    {
        _mainMenu.SetActive(true);
        Time.timeScale = 0;
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
        UpdateGameState(GameState.Menu);
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

    public void StartGame()
    {
        _mainMenu.SetActive(false);
        _gamePlayScreen.SetActive(true);
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        _isGameOver = true;
        UIManager.Instance.ShowGameoverUI(_isGameOver);
        _playerPrefab.SetActive(!_isGameOver);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        _isGameOver = false;
        SceneManager.LoadScene(_activeScene);
        //If player prefab is not instantiated
        UIManager.Instance.ShowGameoverUI(_isGameOver);
        _playerPrefab.SetActive(!_isGameOver);
        //SpawnPlayer();
    }
    public void PauseScreen()
    {
        Time.timeScale = 0;
        _pauseScreen.SetActive(true);
    } 
    public void ResumeGame()
    {
        Debug.Log("Restarted");
        Time.timeScale = 1;
        _pauseScreen.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UpdateGameState(GameState newState)
    {
        _gameStates = newState;

        switch (_gameStates)
        {
            case GameState.Menu:
                break;
            case GameState.MainGame:
                break;
            case GameState.GamePause:
                break;
            case GameState.WinScreen:
                break;
            case GameState.GameOver:
                break;
            case GameState.ShareScreen:
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }



    public enum GameState
    {
        Menu,
        MainGame,
        GamePause,
        WinScreen,
        GameOver,
        ShareScreen
    }
}
