using UnityEngine;

public class ProjectileAnimation : MonoBehaviour
{
    [SerializeField] Animator _anim;
    [SerializeField] AnimationClip _endAnimationClip;
    [SerializeField] AnimationClip _startAnimationClip;
    [SerializeField] AnimationClip _idleAnimationClip;

    public float EndAnimation()
    {
        return PlayAnimation(_endAnimationClip);
    }

    public float StartAnimation()
    {
        return PlayAnimation(_startAnimationClip);
    }

    public float IdleAnimation()
    {
        return PlayAnimation(_idleAnimationClip);
    }

    float PlayAnimation(AnimationClip animation)
    {
        if (animation == null || _anim == null) 
        { 
            Debug.LogWarning("A projectile animation clip reference is missing."); 
            return 0f;
        }
        _anim.Play(animation.name);
        return animation.length;
    }
}
