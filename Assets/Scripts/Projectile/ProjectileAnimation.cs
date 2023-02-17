using UnityEngine;

public class ProjectileAnimation : MonoBehaviour
{
    [SerializeField] Animator _anim;
    [SerializeField] AnimationClip _endAnimationClip;
    [SerializeField] AnimationClip _startAnimationClip;
    [SerializeField] AnimationClip _idleAnimationClip;
    [field: SerializeField] public float AnimationLengthOffset { get; set; }

    public void EndAnimation()
    {
        PlayAnimation(_endAnimationClip);
    }

    public void StartAnimation()
    {
        PlayAnimation(_startAnimationClip);
    }

    public void IdleAnimation()
    {
        PlayAnimation(_idleAnimationClip);
    }

    void PlayAnimation(AnimationClip animation)
    {
        if (!animation || !_anim)
        { 
            Debug.LogWarning("A projectile animation clip or animator reference in Projectile Animation script missing.", this); 
            return;
        }
        _anim.Play(animation.name);
    }
}
