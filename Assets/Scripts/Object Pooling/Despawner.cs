using System.Collections;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    [SerializeField] ProjectilePrefabName _projectileToDespawn;
    [SerializeField] bool _despawn;
    [SerializeField][Min(0)] float _despawningSpeed;

    ObjectPooler _objectPooler;
    bool _isCurrentlyDespawning;
    IEnumerator _despawning;

    public ProjectilePrefabName ProjectileToDespawn
    {
        get { return _projectileToDespawn; }
        set { _projectileToDespawn = value; }
    }

    public bool Despawn
    {
        get { return _despawn;  }
        set { _despawn = value; }
    }

    public float DespawningSpeed
    {
        get { return _despawningSpeed; }
        set { _despawningSpeed = value; }
    }

    void Start()
    {
        _despawn = false;
        _objectPooler = ObjectPooler.Instance;
        _despawning = Despawning();
    }

    void FixedUpdate()
    {
        if (!_despawn || _isCurrentlyDespawning) return;
        if (_despawning != null) { StopCoroutine(_despawning); }
        StartCoroutine(Despawning());
    }

    IEnumerator Despawning(){
        _isCurrentlyDespawning = true;
        while (_despawn)
        {
            if (_despawningSpeed == 0)
                _objectPooler.DespawnAllFromPool(_projectileToDespawn);
            else
                _objectPooler.DespawnFromPool(_projectileToDespawn);
            yield return new WaitForSeconds(_despawningSpeed);
        }
        _isCurrentlyDespawning = false;
    }
}
