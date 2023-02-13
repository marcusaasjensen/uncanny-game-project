using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    [SerializeField] Image _progressBar;
    [SerializeField] float _totalTime;
    [SerializeField] GameOver _gameOver;
    
    
    bool _hasEnded = false;

    float _currentTime = 0;
    float _currentProgress = 0;

    public float TotalTime { get { return _totalTime; } }
    public bool HasEnded { get { return _hasEnded;} }
    
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

        if (_currentTime >= _totalTime)
            _hasEnded = true;
        else
            _hasEnded = false;
        print(_currentTime);
    }

    void RestartProgress() 
    {
        _hasEnded = false;
        _currentTime = 0;
        _currentProgress = 0;
    }

    public float CurrentTime
    {
        get { return _currentTime; }
    }
}
