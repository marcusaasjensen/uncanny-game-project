using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] static bool isGamePaused = false;
    [SerializeField] GameObject pauseMenuUI;

    PlayerInputActions _playerActions;
    InputAction _pauseMenuInput;

    public static bool IsGamePaused { get { return isGamePaused; } }

    void Start()
    {
        if(pauseMenuUI) pauseMenuUI.SetActive(false);
        _playerActions = new PlayerInputActions();
        _pauseMenuInput = _playerActions.Menu.OpenMenu;
        _pauseMenuInput.Enable();
    }

    void Update()
    {
        OnPause();
    }

    void OnPause()
    {
        if (_pauseMenuInput.ReadValue<float>() <= 0.1f || GameOver.IsGameOver || Win.IsLevelFinished) return;
        Pause();
    }

    void Resume() 
    {
        isGamePaused = false;
        if (pauseMenuUI) pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void Pause()
    {
        isGamePaused = true;
        if (pauseMenuUI) pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    void Quit()
    {
        Application.Quit();
        Debug.Log("Application closed.");
    }
}
