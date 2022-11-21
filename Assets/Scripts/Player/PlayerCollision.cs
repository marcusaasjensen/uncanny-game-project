using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Collider2D _collider;

    [Header("Particles")]
    [SerializeField] private ParticleSystem _collisionParticle;
    [SerializeField] private ParticleSystem _damageParticle;

    IEnumerator _doDamage;

    void FixedUpdate() => ColliderOnDashing();

    IEnumerator OnTriggerEnter2D(Collider2D collider)
    {
        if(_playerController == null)
        {
            Debug.LogWarning("Player controller reference inb player collision script is missing.", this);
            yield break;
        }

        if (collider.CompareTag("Damage") && !_playerController._playerLife.IsInvisible && !_playerController._playerMovement.IsDashing)
        {
            ProjectileController proj = collider.GetComponent<ProjectileController>();

            PlayCollisionParticle();

            if(_doDamage != null) yield break;
            _doDamage = _playerController._playerLife.DoDamage(proj.Damage);
            yield return StartCoroutine(_doDamage);
            StopCoroutine(_doDamage);
            _doDamage = null;
        }
    }

    void PlayCollisionParticle()
    {
        if(_collisionParticle != null) _collisionParticle.Play();
        if(_damageParticle != null) _damageParticle.Play();
    }

    void ColliderOnDashing()
    {
        if (_collider != null && _playerController != null)
            _collider.enabled = !_playerController._playerMovement.IsDashing;
    }
}
