using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] Collider2D _collider;
    [SerializeField] List<GameObjectShakeController> _shakeControllers;

    [Header("Particles")]
    [SerializeField] ParticleSystem _collisionParticle;
    [SerializeField] ParticleSystem _damageParticle;

    IEnumerator _doDamage;

    void Awake()
    {
        if (!_playerController)
            _playerController = GetComponent<PlayerController>();
        
        if(!_collider)
            _collider = GetComponent<Collider2D>();
    }

    void FixedUpdate() 
    { 
        ColliderOnEnabling();
    }

    IEnumerator OnTriggerEnter2D(Collider2D collider)
    {
        if(!_playerController)
        {
            Debug.LogWarning("Player controller reference inb player collision script is missing.", this);
            yield break;
        }

        if (collider.CompareTag("Damage"))
        {
            ProjectileController proj = collider.GetComponent<ProjectileController>();

            PlayCollisionParticle();
            ShakeOnCollision();

            if(_doDamage != null) yield break;
            _doDamage = _playerController.PlayerLife.DoDamage(proj.Damage);
            yield return StartCoroutine(_doDamage);
            StopCoroutine(_doDamage);
            _doDamage = null;
        }

        if(collider.CompareTag("Boss"))
        {
            Boss boss = collider.GetComponent<Boss>();
            if (_doDamage != null) yield break;
            _doDamage = _playerController.PlayerLife.DoDamage(boss.DamageOnCollision);
            yield return StartCoroutine(_doDamage);
            StopCoroutine(_doDamage);
            _doDamage = null;
        }
    }

    void PlayCollisionParticle()
    {
        if(_collisionParticle) _collisionParticle.Play();
        if(_damageParticle) _damageParticle.Play();
    }

    void ShakeOnCollision()
    {
        if (_shakeControllers.Count == 0) return;
        foreach (GameObjectShakeController controller in _shakeControllers) 
            controller.Shake();
    }

    void ColliderOnEnabling()
    {
        if (!_collider || !_playerController) return;

        PlayerLife playerLife = _playerController.PlayerLife;
        
        _collider.enabled = !_playerController.PlayerMovement.IsDashing && !playerLife.IsInvisible && !playerLife.IsBeingDamaged;
    }
}
