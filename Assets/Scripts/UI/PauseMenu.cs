using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] static bool isGamePaused = false;
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject settingsMenuUI;
    [SerializeField] Button nextLevelButton;

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

        if (musicSlider) musicSlider.onValueChanged.AddListener(val => ChangeMusicVolumeByTesting(val));
        if (effectSlider) effectSlider.onValueChanged.AddListener(val => ChangeSFXVolumeByTesting(val));

    }

    void Update() => OnPause();

    void ChangeMusicVolumeByTesting(float volume)
    {
        SoundManager.Instance.TestSoundWithVolume(volume);
        SoundManager.Instance.ChangeMusicVolume(volume);
    }

    void ChangeSFXVolumeByTesting(float volume)
    {
        SoundManager.Instance.TestSoundWithVolume(volume);
        SoundManager.Instance.ChangeSFXVolume(volume);
    }

    void OnPause()
    {
        if (_pauseMenuInput.ReadValue<float>() <= 0.1f || GameOver.IsGameOver || Win.IsLevelFinished || isGamePaused) return;
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

        if (!nextLevelButton) return;
        nextLevelButton.interactable = WandererLevelBoss.isLevelCompleted;
    }

    void Restart()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void NextLevel()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void Quit()
    {
        Application.Quit();
        Debug.Log("Application closed.");
    }

    void OpenSettings()
    {
        isGamePaused = true;
        if(pauseMenuUI) pauseMenuUI.SetActive(false);
        if(settingsMenuUI) settingsMenuUI.SetActive(true);
        if(musicSlider) musicSlider.value = SoundManager.Instance.MusicVolume;
        if(effectSlider) effectSlider.value = SoundManager.Instance.SFXVolume;
    }

    public void CloseSettings()
    {
        if(pauseMenuUI) pauseMenuUI.SetActive(true);
        if(settingsMenuUI) settingsMenuUI.SetActive(false);
    }
}
