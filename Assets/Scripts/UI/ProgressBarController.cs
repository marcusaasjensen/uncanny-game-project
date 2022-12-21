using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    [SerializeField] Image _progressBar;
    [SerializeField] float _totalTime;

    float _currentTime = 0;

    public float TotalTime { get { return _totalTime; } }

    void Update() => UpdateProgressBar();

    void UpdateProgressBar()
    {
        if (!_progressBar) return;

        float progress = _currentTime / _totalTime;

        _progressBar.fillAmount = Mathf.Lerp(0, 1, progress);
        
        _currentTime += Time.deltaTime;
    }

    public float CurrentTime
    {
        get { return _currentTime; }
    }
}
