using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] AudioClip _deathSound;

    [SerializeField] PlayerController _playerController;
    [SerializeField] [Min(0)] float _maxHealth = 100f;
    [SerializeField] [Min(0)] float _health;
    [SerializeField] float _timeBeforeNewDamage;
    [SerializeField] Color _damageSpriteColor;
    [SerializeField] float _colorTransitionTime;
    [SerializeField] bool _isInvisible = false;
    [SerializeField] List<AudioClip> _hitSounds;
    [SerializeField] ParticleSystem _deathParticle;

    SpriteRenderer _playerSprite;

    float _currentColorTransitionTime;
    bool _isBeingDamaged = false;

    void Awake()
    {
        if(!_playerSprite)
            _playerSprite = GetComponent<SpriteRenderer>();
        
        if(!_playerController)
            _playerController = GetComponent<PlayerController>();

        _health = _maxHealth;
        _currentColorTransitionTime = _colorTransitionTime;
    }

    public float Health
    {
        get { return _health; }
        set { _health = ClampHealth(_health); }
    }

    public float Maxhealth
    {
        get { return _maxHealth; }
    }

    public bool IsInvisible
    {
        get { return _isInvisible; }
        set { _isInvisible = value; }
    }

    public bool IsBeingDamaged
    {
        get { return _isBeingDamaged; }
    }

    float ClampHealth(float currentHealth)
    {
        return Mathf.Clamp(currentHealth, 0, _maxHealth);
    }

    void Update()
    {
        _health = ClampHealth(_health);
        Die();
        SetPlayerSpriteColor();
    }

    void SetPlayerSpriteColor()
    {
        _playerSprite.color = Color.Lerp(_damageSpriteColor, _playerController.DefaultSpriteColor, _currentColorTransitionTime / _colorTransitionTime);
        _currentColorTransitionTime += Time.deltaTime;
    }

    public void Die()
    {
        if (_health > 0) return;
        SoundManager.Instance.PlaySound(_deathSound);
        PlayDeathParticles();
        gameObject.SetActive(false);
    }

    void PlayDeathParticles()
    {
        if (!_deathParticle) return;
        _deathParticle.transform.position = transform.position;
        _deathParticle.Play();
    }

    public IEnumerator DoDamage(float damage)
    {
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

    void OnDisable()
    {
        _isBeingDamaged = false;
    }

    public void FillHealth()
    {
        _health = _maxHealth;
        print(_health);
    }

}
