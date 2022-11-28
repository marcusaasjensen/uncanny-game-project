using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeController : MonoBehaviour
{

	public CameraShake.CameraShakeProperties testProperties;
    [SerializeField] PlayerLife _player;

    void Update()
    {
        OnPlayerDamage();
    }

    void OnPlayerDamage()
    {
        if (!_player) return;
        if (_player.IsBeingDamaged) Shake();
    }

    [ContextMenu("Shake")]
    void Shake()
    {
        FindObjectOfType<CameraShake>().StartShake(testProperties);
    }
}