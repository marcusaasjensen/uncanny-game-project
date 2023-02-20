using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] static bool isGamePaused = false;
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject settingsMenuUI;

    [Header("Slider volumes")]
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider effectSlider;

    PlayerInputActions _playerActions;
    InputAction _pauseMenuInput;

    public static bool IsGamePaused { get { return isGamePaused; } }

    void Start()
    {
        if (pauseMenuUI) pauseMenuUI.SetActive(false);
        if(settingsMenuUI) settingsMenuUI.SetActive(false);
        _playerActions = new PlayerInputActions();
        _pauseMenuInput = _playerActions.Menu.OpenMenu;
        _pauseMenuInput.Enable();

        if (musicSlider) musicSlider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeMusicVolume(val));
        if (effectSlider) effectSlider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeSFXVolume(val));

    }

    void Update() => OnPause();

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

    void Restart()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Quit()
    {
        Application.Quit();
        Debug.Log("Application closed.");
    }

    void OpenSettings()
    {
        isGamePaused = true;
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
        musicSlider.value = SoundManager.Instance.MusicVolume;
        effectSlider.value = SoundManager.Instance.SFXVolume;
    }

    public void CloseSettings()
    {
        pauseMenuUI.SetActive(true);
        settingsMenuUI.SetActive(false);
    }
}
