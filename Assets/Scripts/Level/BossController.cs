using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [field: SerializeField] public BossAttack AttackController { get; private set; }
    [field: SerializeField] public BossAnimation AnimationController { get; private set; }
    [field: SerializeField] public string Alias { get; private set; }


}
