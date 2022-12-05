using System.Collections.Generic;
using UnityEngine;

public class Clown : MonoBehaviour
{

    [SerializeField] List<AnimationClip> _animations;
    [SerializeField] ProjectileController _starAttack;
    [SerializeField] ProjectileController _balloonAttack;

    public void ThrowStars()
    {
        _starAttack.gameObject.SetActive(true);
    }

    public void ThrowBalloons()
    {
        _balloonAttack.gameObject.SetActive(true);
    }

}
