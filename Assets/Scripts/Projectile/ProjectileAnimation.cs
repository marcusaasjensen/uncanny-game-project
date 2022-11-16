using UnityEngine;

public class ProjectileAnimation : MonoBehaviour
{
    public Animator anim;
    public AnimationClip endAnimationClip;
    public AnimationClip startAnimationClip;
    public AnimationClip idleAnimationClip;

    public float EndAnimation()
    {
        return PlayAnimation(endAnimationClip);
    }

    public float StartAnimation()
    {
        return PlayAnimation(startAnimationClip);
    }

    public float IdleAnimation()
    {
        return PlayAnimation(idleAnimationClip);
    }

    float PlayAnimation(AnimationClip animation)
    {
        if(animation == null || anim == null) return 0f;
        anim.Play(animation.name);
        return animation.length;
    }
}
