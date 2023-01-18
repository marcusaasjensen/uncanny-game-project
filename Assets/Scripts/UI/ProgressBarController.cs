using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    [SerializeField] Image _progressBar;
    [SerializeField] float _totalTime;
    [SerializeField] GameOver _gameOver;

    float _currentTime = 0;
    float _currentProgress = 0;

    public float TotalTime { get { return _totalTime; } }

    void Update() => UpdateProgressBar();

    void UpdateProgressBar()
    {
        if (!_progressBar) return;
        if(_gameOver ? _gameOver.IsGameOver : false) return;
        Progress();
    }

    void Progress() 
    {
        _currentProgress = _currentTime / _totalTime;

        _progressBar.fillAmount = Mathf.Lerp(0, 1, _currentProgress);

        _currentTime += Time.deltaTime;
    }

    void RestartProgress() 
    { 
        _currentTime = 0;
        _currentProgress = 0;
    }

    public float CurrentTime
    {
        get { return _currentTime; }
    }
}
