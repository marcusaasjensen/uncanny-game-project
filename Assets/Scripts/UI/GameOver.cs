using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject _gameOverScreen;
    [SerializeField] Scenario _scenario;

    public bool IsGameOver { get; private set; }

    public void OnGameOver()
    {
        if(_gameOverScreen) _gameOverScreen.SetActive(true);
        if(_scenario) _scenario.StopLevelBoss();
        IsGameOver = true;
    }

    void Start() => LevelEvents.level.OnGameOver += OnGameOver;

    void OnDisable() => LevelEvents.level.OnGameOver -= OnGameOver;
}