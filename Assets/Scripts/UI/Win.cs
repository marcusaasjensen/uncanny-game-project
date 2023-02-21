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
    PlayerInputActions playerActions;

    public static bool IsLevelFinished { get { return isLevelFinished; } }

    void Start()
    {
        gameWinScreen.SetActive(false);
        LevelEvents.level.OnLevelFinished += GameWin;
        playerActions = new PlayerInputActions();
        nextSceneInput = playerActions.Game.NextScene;
        nextSceneInput.Disable();
    }

    void Update()
    {
        OnLevelEnd();
        OnNextScene();
    }

    void OnNextScene()
    {
        if (nextSceneInput == null) return;
        if (nextSceneInput.ReadValue<float>() <= 0.1f) return;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        isLevelFinished = false;
    }

    void OnLevelEnd()
    {
        if (!progressBarController) return;

        if (progressBarController.HasEnded)
            LevelEvents.level.LevelFinished();

        isLevelFinished = progressBarController.HasEnded;
    }

    IEnumerator GameWinCoroutine()
    {
        if (gameWinScreen) gameWinScreen.SetActive(true);
        yield return new WaitForSeconds(timeBeforeNextScene);
        nextSceneInput.Enable();
    }

    void GameWin() 
    {
        StartCoroutine(GameWinCoroutine());
    }

    void OnDisable() => LevelEvents.level.OnLevelFinished -= GameWin;
}
