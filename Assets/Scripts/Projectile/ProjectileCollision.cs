using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]
[RequireComponent(typeof(ProjectileController))]
public class ProjectileCollision : MonoBehaviour
{
    public ProjectileController controller;
    public Collider2D projectileCollider;

    public List<AudioClip> collisionSounds;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (controller == null)
        {
            Debug.LogWarning("Projectile Controller needs to be referenced in Projectile Collision script.");
            return;
        }

        if (!collider.CompareTag(controller.target.tag)) return;

        SoundManager.Instance.PlayRandomSound(collisionSounds, true);

        if (controller.DisappearWhenTouchingTarget)
            gameObject.SetActive(false);
    }

    public void EnableCollider(bool value)
    {
        if (projectileCollider == null) return;
        projectileCollider.enabled = value;
    }
}
