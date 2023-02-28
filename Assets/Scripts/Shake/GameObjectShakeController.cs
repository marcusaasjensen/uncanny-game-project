using UnityEngine;

public class GameObjectShakeController : MonoBehaviour
{
	public GameObjectShake.ShakeProperties testProperties;
    [SerializeField] GameObjectShake _gameObjectToShake;

    [ContextMenu("Shake")]
    public void Shake()
    {
        if (!_gameObjectToShake) return;
        _gameObjectToShake.StartShake(testProperties);
    }
}