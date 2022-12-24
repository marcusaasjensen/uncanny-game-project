using UnityEngine;

public class HealthFillerCheckPoint : MonoBehaviour
{
    [SerializeField] PlayerLife _playerLife;
    [SerializeField] AnimationClip _glowUpAnimation;
    [SerializeField] Animator _anim;
    [SerializeField] ProgressBarController _progressBarController;
    [SerializeField] float _timeOnTimeline;

    const float _PROGRESS_BAR_WIDTH = 2430;

    bool _hasBeenActivated = false;

    void Awake()
    {
        if (!_anim)
            _anim = GetComponent<Animator>();

        SetTimeOnTimeline();
    }

    void Update() => UpdateCheckPoint();

    void SetTimeOnTimeline()
    {
        if (!_progressBarController) return;
        _timeOnTimeline = Mathf.Lerp(0, _progressBarController.TotalTime, (GetComponent<RectTransform>().anchoredPosition.x + 1167) / _PROGRESS_BAR_WIDTH);
    }

    void UpdateCheckPoint()
    {
        if (_hasBeenActivated) return;
        if (!_progressBarController) return;
        if (_progressBarController.CurrentTime < _timeOnTimeline) return;
        
        ActivateHealthFillerCheckPoint();
    }

    void ActivateHealthFillerCheckPoint()
    {
        if (_playerLife.Health <= 0) return;

        if (_glowUpAnimation && _anim)
            _anim.Play(_glowUpAnimation.name);

        if (_playerLife)
            _playerLife.FillHealth();

        _hasBeenActivated = true;
    }
}

