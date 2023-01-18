using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject _gameOverScreen;

    public bool IsGameOver { get; private set; }

    public void OnGameOver()
    {
        _gameOverScreen.SetActive(true);
        IsGameOver = true;
    }

    void Start() => LevelEvents.level.OnGameOver += OnGameOver;

    void OnDisable() => LevelEvents.level.OnGameOver -= OnGameOver;
}