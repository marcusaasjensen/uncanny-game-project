using UnityEngine;

public interface IPooledObject<T> where T : MonoBehaviour {
    void OnObjectSpawn(T obj);
}