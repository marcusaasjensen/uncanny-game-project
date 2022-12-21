using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] Image _frontHealthBar;
    [SerializeField] Image _backHealthBar;
    [SerializeField] Image _backgroundHealthBar;
    [SerializeField] PlayerLife _player;
    [SerializeField] [Min(0)] float _damageChipSpeed = 2f;
    [SerializeField][Min(0)] float _healChipSpeed = 4f;

    float _lerpTimer = 0f;
    float _newHealth = 0;

    void Update()
    {
        SetNewHealth();
        UpdateHealthUI();
        OnPLayerDeath();
        OnPlayerOverlaps();
    }

    void SetNewHealth()
    {
        if (_newHealth < _player.Health)
        {
            _lerpTimer = 0f;
            _newHealth = _player.Health;
        }
    }

    void OnPLayerDeath()
    {
        if (!_player || _player.Health > 0) return;
        _backHealthBar.fillAmount = 0;
        _frontHealthBar.fillAmount = 0;
    }

    void OnPlayerOverlaps()
    {
        if (_player.transform.position.x < -4 && _player.transform.position.y > 4)
        {
            _frontHealthBar.CrossFadeAlpha(.1f, .1f, false);
            _backHealthBar.CrossFadeAlpha(.1f, .1f, false);
            _backgroundHealthBar.CrossFadeAlpha(.1f, .1f, false);
        }
        else
        {
            _frontHealthBar.CrossFadeAlpha(1f, .2f, false);
            _backHealthBar.CrossFadeAlpha(1f, .2f, false);
            _backgroundHealthBar.CrossFadeAlpha(1f, .2f, false);
        }
    }


    public void UpdateHealthUI()
    {
        if (!_backHealthBar || !_frontHealthBar)
        {
            Debug.LogWarning("An Health Bar Image reference in the healthBarController script is missing.");
            return;
        }
        float fillA = _frontHealthBar.fillAmount;
        float fillB = _backHealthBar.fillAmount;

        float healthFraction = _player.Health / _player.Maxhealth;

        if(_newHealth != healthFraction)
        {
            _newHealth = healthFraction;
            _lerpTimer = 0f;
        }

        if (fillB > healthFraction)
        {
            _frontHealthBar.fillAmount = healthFraction;
            _backHealthBar.color = Color.red;
            _lerpTimer += Time.deltaTime;
            float percentComplete = _lerpTimer / _damageChipSpeed;
            _backHealthBar.fillAmount = Mathf.Lerp(fillB, healthFraction, percentComplete);
        }

        if (fillA < healthFraction)
        {
            _backHealthBar.fillAmount = healthFraction;
            _backHealthBar.color = Color.white;
            _lerpTimer += Time.deltaTime;
            float percentComplete = _lerpTimer / _healChipSpeed;
            _frontHealthBar.fillAmount = Mathf.Lerp(fillA, healthFraction, percentComplete);
        }

    }
}
