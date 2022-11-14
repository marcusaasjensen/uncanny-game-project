using UnityEngine;

[RequireComponent(typeof(SpawningBehaviour))]
public class SpawnerController : ProjectileController, IPooledObject<SpawnerController>
{
    public SpawningBehaviour spawningBehaviour;
    [Tooltip("Necessary spawner template reference for affecting continuously the spawner if continuously affected.")]
    public SpawnerController spawnerTMP;

    ObjectPooler _objectPooler;

    public void OnObjectSpawn(SpawnerController spawner){
        spawnerTMP = spawner;

        base.OnObjectSpawn(spawner);
        SetSpawningBehaviour(spawner.spawningBehaviour);
    }

    public void SetSpawningBehaviour(SpawningBehaviour tmp)
    {
        spawningBehaviour.projectileToSpawn = tmp.projectileToSpawn;
        spawningBehaviour.projectileToSpawnConfig = tmp.projectileToSpawnConfig;
        spawningBehaviour.IsSpawning = tmp.IsSpawning;
        spawningBehaviour.NumberOfProjectiles = tmp.NumberOfProjectiles;
        spawningBehaviour.TimeBetweenSpawns = tmp.TimeBetweenSpawns;
        spawningBehaviour.SpawningRange = tmp.SpawningRange;
        spawningBehaviour.StopWhenNotVisible = tmp.StopWhenNotVisible;
        spawningBehaviour.ActivateWhenVisible = tmp.ActivateWhenVisible;
        spawningBehaviour.TimeBeforeActivating = tmp.TimeBeforeActivating;
        spawningBehaviour.RandomizeDirection = tmp.RandomizeDirection;
        spawningBehaviour.RandomizeSpeed = tmp.RandomizeSpeed;
        spawningBehaviour.RandomizePosition = tmp.RandomizePosition;
        spawningBehaviour.RandomizeSize = tmp.RandomizeSize;
        spawningBehaviour.NumberOfScopes = tmp.NumberOfScopes;
        spawningBehaviour.ScopeRange = tmp.ScopeRange;
        spawningBehaviour.NumberOfSides = tmp.NumberOfSides;
        spawningBehaviour.SideBending = tmp.SideBending;
        spawningBehaviour.IsTargetting = tmp.IsTargetting;
        spawningBehaviour.TargettingSpeed = tmp.TargettingSpeed;
        spawningBehaviour.SpawningOnRhythm = tmp.SpawningOnRhythm;
        spawningBehaviour.rhythm = tmp.rhythm;
        spawningBehaviour.SpawnerDirectionAffectsProjectile = tmp.SpawnerDirectionAffectsProjectile;
    }

    public ObjectPooler ObjectPooler
    {
        get { return _objectPooler; }
        set { if (_objectPooler != value) { _objectPooler = value; } }
    }

    void Start() => _objectPooler = ObjectPooler.Instance;

    void Update() => ContinuouslyAffectedToggler();
    
    void ContinuouslyAffectedToggler() {
        if (spawnerTMP == null || !isContinuouslyAffected) return;

        SetProjectile(spawnerTMP);
        SetSpawningBehaviour(spawnerTMP.spawningBehaviour);
    }
}