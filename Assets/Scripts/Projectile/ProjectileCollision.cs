using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]
[RequireComponent(typeof(ProjectileController))]
public class ProjectileCollision : MonoBehaviour
{
    [SerializeField] ProjectileController _projectileController;
    [SerializeField] Collider2D _projectileCollider;
    [SerializeField] List<AudioClip> _collisionSounds;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (_projectileController == null)
        {
            Debug.LogWarning("Projectile Controller needs to be referenced in Projectile Collision script.", this);
            return;
        }

        if (!collider.CompareTag(_projectileController.Target.tag)) return; //fix no collision when player is dashing

        SoundManager.Instance.PlayRandomSound(_collisionSounds, true);

        if (_projectileController.DisappearWhenTouchingTarget)
            gameObject.SetActive(false);
    }

    public void EnableCollider(bool value)
    {
        if (_projectileCollider == null) return;
        _projectileCollider.enabled = value;
    }
}
