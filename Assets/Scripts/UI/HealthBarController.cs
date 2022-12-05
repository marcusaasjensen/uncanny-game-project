using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] Image _frontHealthBar;
    [SerializeField] Image _backHealthBar;
    [SerializeField] PlayerLife _player;
    [SerializeField] [Min(0)] float _chipSpeed = 2f;

    float _lerpTimer = 0f;

    void Update()
    {
        UpdateHealthUI();
        OnPlayerDamage();
        OnPLayerDeath();
    }

    void OnPLayerDeath()
    {
        if (!_player || _player.Health > 0) return;
        _backHealthBar.fillAmount = 0;
        _frontHealthBar.fillAmount = 0;
    }

    void OnPlayerDamage()
    {
        if(_player.IsBeingDamaged)
            _lerpTimer = 0f;
    }


    public void UpdateHealthUI()
    {
        if (!_backHealthBar || !_frontHealthBar)
        {
            Debug.LogWarning("An Health Bar Image reference in the healthBarController script is missing.");
            return; 
        }
        
        float fillB = _backHealthBar.fillAmount;
        float healthFraction = _player.Health / _player.Maxhealth;
        if (fillB > healthFraction)
        {
            _frontHealthBar.fillAmount = healthFraction;
            _backHealthBar.color = Color.red;
            _lerpTimer += Time.deltaTime;
            float percentComplete = _lerpTimer / _chipSpeed;
            _backHealthBar.fillAmount = Mathf.Lerp(fillB, healthFraction, percentComplete);
        }
    }
}
