using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    public Image frontHealthBar;
    public Image backHealthBar;
    public AudioClip deathSound;

    [SerializeField] internal PlayerController _playerController;
    [SerializeField] private float _health;
    [SerializeField] private const float _maxHealth = 100f;
    [SerializeField] private float _timeBeforeNewDamage;
    [SerializeField] private Color _damageSpriteColor;
    [SerializeField] float _colorTransitionTime;
    [SerializeField] private bool _isInvisible = false;
    public List<AudioClip> _hitSounds;

    SpriteRenderer _playerSprite;

    float _currentColorTransitionTime;
    float _lerpTimer;
    float _chipSpeed = 2f;
    bool _isBeingDamaged;

    void Awake()
    {
        _playerSprite = GetComponent<SpriteRenderer>();
        _playerController = GetComponent<PlayerController>();
        _health = _maxHealth;
        _isBeingDamaged = false;
        _currentColorTransitionTime = _colorTransitionTime;
    }

    public float Health
    {
        get { return _health; }
        set
        {
            if (value >= 0)
                _health = value;
            else
                _health = 0;
        }
    }

    public bool IsInvisible
    {
        get { return _isInvisible; }
        set { _isInvisible = value; }
    }


    // Update is called once per frame
    void Update()
    {
        _health = Mathf.Clamp(_health, 0, _maxHealth);
        PlayerDeath();
        if(backHealthBar != null && frontHealthBar != null)
            UpdateHealthUI();
        SetPlayerSpriteColor();
    }

    void SetPlayerSpriteColor()
    {
        _playerSprite.color = Color.Lerp(_damageSpriteColor, _playerController.DefaultSpriteColor, _currentColorTransitionTime / _colorTransitionTime);
        _currentColorTransitionTime += Time.deltaTime;
    }

    public void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float healthFraction = _health/ _maxHealth;
        if(fillB > healthFraction)
        {
            frontHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.red;
            _lerpTimer += Time.deltaTime;
            float percentComplete = _lerpTimer / _chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB,healthFraction,percentComplete);
        }
    }

    public void PlayerDeath()
    {
        if (_health > 0) return;
        backHealthBar.fillAmount = 0;
        frontHealthBar.fillAmount = 0;
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
        gameObject.SetActive(false);
    }

    public IEnumerator DoDamage(float damage)
    {
        _lerpTimer = 0f;
        if (!_isBeingDamaged)
        {
            SoundManager.Instance.PlayRandomSound(_hitSounds, true);
            _currentColorTransitionTime = 0;
            _health -= damage;
            _isBeingDamaged = true;

        }
        yield return new WaitForSeconds(_timeBeforeNewDamage);
        _isBeingDamaged = false;
    }
}
