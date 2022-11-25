using UnityEngine;

[RequireComponent(typeof(SpawningBehaviour))]
public class SpawnerController : ProjectileController, IPooledObject<SpawnerController>
{
    [SerializeField] SpawningBehaviour _spawningBehaviour;
    [Tooltip("Necessary spawner configuration reference for affecting continuously the spawner if continuously affected.")]
    [SerializeField] SpawnerController _spawnerConfiguration;

    ObjectPooler _objectPooler;

    void Awake() 
    {
        if(!_spawningBehaviour)
            _spawningBehaviour = gameObject.GetComponent<SpawningBehaviour>();

        if (!_spawnerConfiguration && base.projectileName == ProjectilePrefabName.SpawnerConfig)
            _spawnerConfiguration = GetComponent<SpawnerController>();

        if (!_objectPooler)
            _objectPooler = ObjectPooler.Instance;
    }

    public void OnObjectSpawn(SpawnerController spawner)
    {
        _spawnerConfiguration = spawner;

        base.OnObjectSpawn(spawner);
        SetSpawningBehaviour(spawner.SpawningBehaviour);
    }

    public SpawningBehaviour SpawningBehaviour
    {
        get 
        { 
            if(_spawningBehaviour) 
                return _spawningBehaviour;
            else
                Debug.LogWarning($"Spawning Behaviour reference in Spawner Controller script is missing.", this);
            return null;
        }
        set { _spawningBehaviour = value; }
    }

    public SpawnerController SpawnerConfiguration
    {
        get 
        {
            if (_spawnerConfiguration)
                return _spawnerConfiguration;
            else
                Debug.LogWarning("The Spawner Controller reference (as Spawner Configuration) in Spawner Controller script is missing.", this);
            return null;
        }
        set { _spawnerConfiguration = value; }
    }

    public ObjectPooler ObjectPooler
    {
        get 
        {
            if (!_objectPooler)
                _objectPooler = ObjectPooler.Instance;    
            return _objectPooler; 
        }
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
        _spawningBehaviour.SpawnerDirectionAffectsProjectile = tmp.SpawnerDirectionAffectsProjectile;
    }

    void Update() => ContinuouslyAffectedToggler();
    
    void ContinuouslyAffectedToggler() {
        if (!_spawnerConfiguration || !realTimeConfiguration) return;

        SetProjectile(_spawnerConfiguration);
        SetSpawningBehaviour(_spawnerConfiguration.SpawningBehaviour);
    }
}