using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [field: SerializeField] public BossAttacks AttackController { get; private set; }
    [field: SerializeField] public BossAnimation AnimationController { get; private set; }
    [field: SerializeField] public string Alias { get; private set; }

    void Awake()
    {
        if (!AttackController)
            AttackController = GetComponent<BossAttacks>();

        if (!AnimationController)
            AnimationController = GetComponent<BossAnimation>();
    }
}
