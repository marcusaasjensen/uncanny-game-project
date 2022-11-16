using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private PlayerController PlayerController;
    [SerializeField] private ParticleSystem _collisionParticle;
    [SerializeField] private ParticleSystem _damageParticle;

    IEnumerator _doDamage;

    void Awake()
    {
        PlayerController = GetComponent<PlayerController>();
    }

    IEnumerator OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.CompareTag("Damage") && !PlayerController._playerLife.IsInvisible && !PlayerController._playerMovement.IsDashing)
        {
            ProjectileController proj = collider.GetComponent<ProjectileController>();

            PlayCollisionParticle();

            if(_doDamage != null) yield break;
            _doDamage = PlayerController._playerLife.DoDamage(proj.Damage);
            yield return StartCoroutine(_doDamage);
            StopCoroutine(_doDamage);
            _doDamage = null;
        }
    }

    void PlayCollisionParticle()
    {
        _collisionParticle.Play();
        _damageParticle.Play();
    }
}
