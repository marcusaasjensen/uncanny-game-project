using UnityEngine;

[RequireComponent(typeof(SpawningBehaviour))]
public class SpawnerController : ProjectileController, IPooledObject<SpawnerController>
{
    [SerializeField] SpawningBehaviour _spawningBehaviour;
    [Tooltip("Necessary spawner configuration reference for affecting continuously the spawner if continuously affected.")]
    [SerializeField] SpawnerController _spawnerConfiguration;

    ObjectPooler _objectPooler;

    public void OnObjectSpawn(SpawnerController spawner){
        _spawnerConfiguration = spawner;

        base.OnObjectSpawn(spawner);
        SetSpawningBehaviour(spawner._spawningBehaviour);
    }

    public SpawningBehaviour SpawningBehaviour
    {
        get { return _spawningBehaviour; }
        set { _spawningBehaviour = value; }
    }

    public SpawnerController SpawnerConfiguration
    {
        get { return _spawnerConfiguration; }
        set { _spawnerConfiguration = value; }
    }


    public ObjectPooler ObjectPooler
    {
        get { return _objectPooler; }
        set { if (_objectPooler != value) { _objectPooler = value; } }
    }

    public void SetSpawningBehaviour(SpawningBehaviour tmp)
    {
        _spawningBehaviour.ProjectileToSpawn = tmp.ProjectileToSpawn;
        _spawningBehaviour.ProjectileToSpawnConfig = tmp.ProjectileToSpawnConfig;
        _spawningBehaviour.IsSpawning = tmp.IsSpawning;
        _spawningBehaviour.NumberOfProjectiles = tmp.NumberOfProjectiles;
        _spawningBehaviour.TimeBetweenSpawns = tmp.TimeBetweenSpawns;
        _spawningBehaviour.SpawningRange = tmp.SpawningRange;
        _spawningBehaviour.StopWhenNotVisible = tmp.StopWhenNotVisible;
        _spawningBehaviour.ActivateWhenVisible = tmp.ActivateWhenVisible;
        _spawningBehaviour.TimeBeforeActivating = tmp.TimeBeforeActivating;
        _spawningBehaviour.RandomizeDirection = tmp.RandomizeDirection;
        _spawningBehaviour.RandomizeSpeed = tmp.RandomizeSpeed;
        _spawningBehaviour.RandomizePosition = tmp.RandomizePosition;
        _spawningBehaviour.RandomizeSize = tmp.RandomizeSize;
        _spawningBehaviour.NumberOfScopes = tmp.NumberOfScopes;
        _spawningBehaviour.ScopeRange = tmp.ScopeRange;
        _spawningBehaviour.NumberOfSides = tmp.NumberOfSides;
        _spawningBehaviour.SideBending = tmp.SideBending;
        _spawningBehaviour.IsTargetting = tmp.IsTargetting;
        _spawningBehaviour.TargettingSpeed = tmp.TargettingSpeed;
        _spawningBehaviour.SpawningOnRhythm = tmp.SpawningOnRhythm;
        _spawningBehaviour.Rhythm = tmp.Rhythm;
        _spawningBehaviour.SpawnerDirectionAffectsProjectile = tmp.SpawnerDirectionAffectsProjectile;
    }

    void Start() => _objectPooler = ObjectPooler.Instance;

    void Update() => ContinuouslyAffectedToggler();
    
    void ContinuouslyAffectedToggler() {
        if (_spawnerConfiguration == null || !realTimeConfiguration) return;

        SetProjectile(_spawnerConfiguration);
        SetSpawningBehaviour(_spawnerConfiguration._spawningBehaviour);
    }
}