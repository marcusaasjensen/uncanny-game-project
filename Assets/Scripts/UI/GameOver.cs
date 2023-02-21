using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject _gameOverScreen;
    [SerializeField] LevelProgression _levelProgression;
    [SerializeField] float _timeBeforeRestarting;
    [SerializeField] static bool isGameOver;
    PlayerInputActions playerActions;
    InputAction _restartInput;

    public static bool IsGameOver { get { return isGameOver; } }

    public void OnGameOver()
    {
        isGameOver = true;
        StartCoroutine(GameOverCoroutine());
    }

    IEnumerator GameOverCoroutine()
    {
        if (_gameOverScreen) _gameOverScreen.SetActive(true);
        if (_levelProgression) _levelProgression.StopLevelBoss();
        yield return new WaitForSeconds(_timeBeforeRestarting);
        _restartInput.Enable();
    }

    void Start()
    {
        isGameOver = false;
        LevelEvents.level.OnGameOver += OnGameOver;
        playerActions = new PlayerInputActions();
        _restartInput = playerActions.Game.Restart;
        _restartInput.Disable();
    }

    void Update() => OnRestart();

    void OnRestart()
    {
        if (_restartInput.ReadValue<float>() <= 0.1f) return;
        Restart();
    }

    void Restart()
    {
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnDisable() => LevelEvents.level.OnGameOver -= OnGameOver;
}