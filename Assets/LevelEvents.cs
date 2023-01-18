using System;
using UnityEngine;

public class LevelEvents : MonoBehaviour
{
    public static LevelEvents level;

    public event Action OnGameOver;
    public event Action OnLevelFinished;

    void Awake() => level = this;

    public void GameOver() => OnGameOver?.Invoke();
    public void LevelFinished() => OnLevelFinished?.Invoke();

}
