using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject _gameOverScreen;
    [SerializeField] Scenario _scenario;
    [SerializeField] float _timeBeforeRestarting;
    PlayerInputActions playerActions;
    InputAction _restartInput;

    public bool IsGameOver { get; private set; }

    public void OnGameOver()
    {
        IsGameOver = true;
        StartCoroutine(GameOverCoroutine());
    }

    IEnumerator GameOverCoroutine()
    {
        if (_gameOverScreen) _gameOverScreen.SetActive(true);
        if (_scenario) _scenario.StopLevelBoss();
        yield return new WaitForSeconds(_timeBeforeRestarting);
        _restartInput.Enable();
    }

    void Start()
    {
        LevelEvents.level.OnGameOver += OnGameOver;
        playerActions = new PlayerInputActions();
        _restartInput = playerActions.Game.Restart;
        _restartInput.Disable();
    }

    void Update()
    {
        if(_restartInput.ReadValue<float>() > 0.1f)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnDisable() => LevelEvents.level.OnGameOver -= OnGameOver;
}