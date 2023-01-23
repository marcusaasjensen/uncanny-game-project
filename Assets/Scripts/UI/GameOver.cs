using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject _gameOverScreen;
    [SerializeField] Scenario _scenario;
    PlayerInputActions playerActions;
    InputAction _restartInput;

    public bool IsGameOver { get; private set; }

    public void OnGameOver()
    {
        if(_gameOverScreen) _gameOverScreen.SetActive(true);
        if(_scenario) _scenario.StopLevelBoss();
        IsGameOver = true;
        _restartInput.Enable();
    }

    void Start()
    {
        LevelEvents.level.OnGameOver += OnGameOver;
        playerActions = new PlayerInputActions();
        _restartInput = playerActions.Game.Restart;
        _restartInput.Disable();
    }

    void OnDisable() => LevelEvents.level.OnGameOver -= OnGameOver;
}