using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [field:SerializeField] public string Alias { get; private set; }
    [field: SerializeField] public float DamageOnCollision { get; private set; }
}
