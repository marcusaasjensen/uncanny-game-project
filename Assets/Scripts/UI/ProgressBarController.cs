using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    [SerializeField] Image _progressBar;
    [SerializeField] TimelineAsset _levelTimeline;
    
    bool _hasEnded = false;

    float _currentTime = 0;
    float _currentProgress = 0;

    public float TotalTime { get { return (float) _levelTimeline.duration; } }
    public bool HasEnded { get { return _hasEnded;} }
    
    void Update() => UpdateProgressBar();

    void UpdateProgressBar()
    {
        if (!_progressBar) return;
        if(GameOver.IsGameOver) return;
        Progress();
    }

    void Progress() 
    {
        float totalTime = (float) _levelTimeline.duration;

        _currentProgress = _currentTime / totalTime;

        _progressBar.fillAmount = Mathf.Lerp(0, 1, _currentProgress);

        _currentTime += Time.deltaTime;

        if (_currentTime >= totalTime)
            _hasEnded = true;
        else
            _hasEnded = false;
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
