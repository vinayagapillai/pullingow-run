using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject _playerPrefab;

    public static GameManager Instance;
    
    public int Score { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    //public void UpdateScore(int value)
    //{
    //    Score += value;
    //    UIManager.Instance.UpdateScoreUI(Score);
    //}
    
    //public void SpawnPlayer()
    //{
    //    GameObject spawnedPlayer = GameObject.Instantiate(_playerPrefab);
    //    spawnedPlayer.transform.name = "Player";
    //}
}
