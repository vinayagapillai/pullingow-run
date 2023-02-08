using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    GameObject _gameOverCanvas;

    public void StopGame(int score)
    {
        _gameOverCanvas.SetActive(true);
    }  
}
