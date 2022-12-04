using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    [SerializeField] Image _progressBar;
    [SerializeField] float _totalTime;
    
    float _currentTime = 0;

    void Update()
    {
        UpdateProgressBar();
    }

    void UpdateProgressBar()
    {
        if (_progressBar == null) return;

        float progress = _currentTime / _totalTime;

        _progressBar.fillAmount = Mathf.Lerp(0, 1, progress);
        
        _currentTime += Time.deltaTime;
    }
}
