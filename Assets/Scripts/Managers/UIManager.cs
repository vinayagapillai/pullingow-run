using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject _gameplayUI;
    [SerializeField] private GameObject _gameoverUI;
    [SerializeField] private TMP_Text _scoreUI;

    private int _total = 0;
    //[SerializeField] private TMP_Text _scoreUI;

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

    public void ShowGameplayUI(bool active)
    {
        _gameplayUI.SetActive(active);
    }

    public void ShowGameoverUI(bool active)
    {
        _gameoverUI.SetActive(active);
    }

    public void UpdateScoreUI(int value)
    {
        _total += value;
        _scoreUI.SetText(_total.ToString());
    }
}
