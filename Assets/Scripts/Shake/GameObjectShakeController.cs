using UnityEngine;

public class GameObjectShakeController : MonoBehaviour
{

	public GameObjectShake.ShakeProperties testProperties;
    [SerializeField] GameObjectShake _gameObjectToshake;

    [ContextMenu("Shake")]
    public void Shake()
    {
        if (!_gameObjectToshake) return;
        _gameObjectToshake.StartShake(testProperties);
    }
}