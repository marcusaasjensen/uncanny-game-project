using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    [field: SerializeField] public AnimationClip Walk { get; private set; }
    [field: SerializeField] public AnimationClip Idle { get; private set; }
    [field: SerializeField] public AnimationClip Dance { get; private set; }
    //...

}
