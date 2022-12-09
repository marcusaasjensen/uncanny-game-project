using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerHint : MonoBehaviour
{
    [field: SerializeField] public Image HintImage { get; private set; }
    [field: SerializeField] public float _radius { get; private set; }
    [field: SerializeField] public float LifeTime { get; private set; }

    [Header("Animation")]
    [SerializeField] Animator _animator;
    [SerializeField] AnimationClip _startAnimation;
    [SerializeField] AnimationClip _idleAnimation;
    [SerializeField] AnimationClip _endAnimation;

    public IEnumerator Appear(ProjectileMovement projectile)
    {
        yield return null;
    }

    void Update()
    {
        
    }
}
