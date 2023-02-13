using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Win : MonoBehaviour
{
    [SerializeField] ProgressBarController progressBarController;
    [SerializeField] GameObject gameWinScreen;
    [SerializeField] float timeBeforeNextScene;
    InputAction nextSceneInput;
    PlayerInputActions playerActions;

    void Start() 
    {
        LevelEvents.level.OnLevelFinished += GameWin;
        playerActions = new PlayerInputActions();
        nextSceneInput = playerActions.Game.NextScene;
        nextSceneInput.Disable();
    }

    void Update()
    {
        OnLevelEnd();

        if(nextSceneInput.ReadValue<float>() > 0.1f)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);


    }

    void OnLevelEnd()
    {
        if (!progressBarController) return;

        if (progressBarController.HasEnded)
            LevelEvents.level.LevelFinished();
    }

    IEnumerator GameWinCoroutine()
    {
        if (gameWinScreen) gameWinScreen.SetActive(true);
        yield return new WaitForSeconds(timeBeforeNextScene);
        nextSceneInput.Enable();
    }

    void GameWin() 
    {
        print("Level win!");
        StartCoroutine(GameWinCoroutine());
    }


    void OnDisable() => LevelEvents.level.OnLevelFinished -= GameWin;
}
