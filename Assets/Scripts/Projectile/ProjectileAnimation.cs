using UnityEngine;

public class ProjectileAnimation : MonoBehaviour
{
    public Animator anim;
    public AnimationClip endAnimationClip;
    public AnimationClip startAnimationClip;
    public AnimationClip idleAnimationClip;

    public float EndAnimation()
    {
        if (endAnimationClip == null || anim == null) return 0f;
        anim.Play(endAnimationClip.name);
        return idleAnimationClip.length;
    }

    public float StartAnimation()
    {
        if (startAnimationClip == null || anim == null) return 0f;
        anim.Play(startAnimationClip.name);
        return startAnimationClip.length;
    }

    public float IdleAnimation()
    {
        if (idleAnimationClip == null || anim == null) return 0f;
        anim.Play(idleAnimationClip.name);
        return idleAnimationClip.length;
    }
}
