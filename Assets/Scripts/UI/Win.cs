using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Win : MonoBehaviour
{
    [SerializeField] ProgressBarController progressBarController;
    [SerializeField] GameObject gameWinScreen;
    [SerializeField] float timeBeforeNextScene;
    [SerializeField] static bool isLevelFinished = false;
    InputAction nextSceneInput;
    InputAction restartAfterWinInput;
    PlayerInputActions playerActions;

    public static bool IsLevelFinished { get { return isLevelFinished; } }

    void Start()
    {
        gameWinScreen.SetActive(false);
        LevelEvents.level.OnLevelFinished += GameWin;
        playerActions = new PlayerInputActions();
        nextSceneInput = playerActions.Game.NextScene;
        restartAfterWinInput = playerActions.Game.WinRestart;
        restartAfterWinInput.Disable();
        nextSceneInput.Disable();
    }

    void Update() => OnLevelEnd();

    void OnNextScene()
    {
        if (nextSceneInput == null) return;
        if (nextSceneInput.ReadValue<float>() <= 0.1f) return;
        isLevelFinished = false;
        LevelEvents.level.OnLevelFinished -= GameWin;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void OnRestartScene()
    {
        if (restartAfterWinInput == null) return;
        if (restartAfterWinInput.ReadValue<float>() <= 0.1f) return;
        isLevelFinished = false;
        LevelEvents.level.OnLevelFinished -= GameWin;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnLevelEnd()
    {
        if (!progressBarController) return;

        if (progressBarController.HasEnded)
            LevelEvents.level.LevelFinished();

        isLevelFinished = progressBarController.HasEnded;

        OnNextScene();
        OnRestartScene();
    }

    IEnumerator GameWinCoroutine()
    {
        if (gameWinScreen) gameWinScreen.SetActive(true);
        yield return new WaitForSeconds(timeBeforeNextScene);
        nextSceneInput.Enable();
        restartAfterWinInput.Enable();
    }

    void GameWin() 
    {
        StartCoroutine(GameWinCoroutine());
    }
}
