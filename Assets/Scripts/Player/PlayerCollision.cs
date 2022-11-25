using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] Collider2D _collider;

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

    void FixedUpdate() => ColliderOnDashing();

    IEnumerator OnTriggerEnter2D(Collider2D collider)
    {
        if(!_playerController)
        {
            Debug.LogWarning("Player controller reference inb player collision script is missing.", this);
            yield break;
        }

        if (collider.CompareTag("Damage") && !_playerController.PlayerLife.IsInvisible && !_playerController.PlayerMovement.IsDashing)
        {
            ProjectileController proj = collider.GetComponent<ProjectileController>();

            PlayCollisionParticle();

            if(_doDamage != null) yield break;
            _doDamage = _playerController.PlayerLife.DoDamage(proj.Damage);
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

    void ColliderOnDashing()
    {
        if (!_collider && !_playerController)
            _collider.enabled = !_playerController.PlayerMovement.IsDashing;
    }
}
