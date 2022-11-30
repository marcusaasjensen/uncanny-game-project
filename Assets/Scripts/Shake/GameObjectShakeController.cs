using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectShakeController : MonoBehaviour
{

	public GameObjectShake.ShakeProperties testProperties;
    [SerializeField] GameObjectShake _gameObjectToshake;
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
        if (!_gameObjectToshake) return;
        _gameObjectToshake.StartShake(testProperties);
    }
}