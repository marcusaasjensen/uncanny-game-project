using System.Collections;
using UnityEngine;
using static UnityEngine.Mathf;

[DisallowMultipleComponent]
public class SpawningBehaviour : MonoBehaviour
{
    [SerializeField] SpawnerController _spawnerController;

    [Header("Projectile To Spawn")]
    [Tooltip("Reference of a projectile configuration with which the parameters of the projectile to spawn are going to be affected.")]
    [SerializeField] ProjectileController _projectileToSpawnConfig;
    [Tooltip("Name of the actual projectile (prefab) that is going to spawn.")]
    [SerializeField] ProjectilePrefabName _projectilePrefabToSpawn;
    [Tooltip("If true, the spawner's direction will affect projectile's direction.")]
    [SerializeField] bool _spawnerDirectionAffectsProjectile;

    [Header("Timing")]
    [Tooltip("Time between each spawning of a projectile.")]
    [SerializeField][Min(0)] float _timeBetweenSpawns;
    [Tooltip("If set to true, the time between each spawn will depends on the rhythm of the recording player.")]
    [SerializeField] bool _spawningOnRhythm;

    [Header("Spawning Direction")]
    [Tooltip("Number of projectiles to spawn at the same time at every 'spawning speed'.")]
    [SerializeField] int _numberOfProjectilesPerSpawn;
    [Tooltip("Range at which the spawner spawns projectiles.")]
    [SerializeField] [Range(0,1)] float _spawningRange = 1f;
    [Tooltip("Allows you to have multiple ranges of spawning (scope) you can control.")]
    [SerializeField][Min(1)] int _numberOfScopes;
    [Tooltip("Range at which each scopes are going to spaw projectiles.")]
    [SerializeField] [Range(0,1)] float _scopeRange;

    [Header("Radial Shape")]
    [SerializeField] int _numberOfSides;
    [Tooltip("How much a side of the shape bends in terms of each projectile move speed.")]
    [SerializeField] [Range(0, 10)] float _sideBending;

    [Header("Randomizing Projectiles")]
    [SerializeField] bool _randomizePosition;
    [SerializeField] bool _randomizeDirection;
    [SerializeField] bool _randomizeSpeed;
    [SerializeField] bool _randomizeSize;

    [Header("Spawning")]
    [Tooltip("Time before the active spawner starts to spawn projectiles.")]
    [SerializeField] float _timeBeforeActivating;
    [SerializeField] bool _isSpawning;
    [SerializeField] bool _stopWhenNotVisible;
    [SerializeField] bool _activateWhenVisible;

    [Header("Targetting")]
    [SerializeField] bool _isTargetting;
    [SerializeField] float _targettingSpeed;

    float _rotationOffset;
    bool _hasStartedSpawning;

    public SpawnerController SpawnerController
    {
        get 
        {
            if (_spawnerController)
                return _spawnerController;
            else
                Debug.LogWarning("The Spawner Controller reference in the Spawning Behabiour script is missing.", this);
            return null;
        }
        set { _spawnerController = value; }
    }     

    public ProjectileController ProjectileToSpawnConfig
    {
        get
        {
            if (_projectileToSpawnConfig)
                return _projectileToSpawnConfig;
            else
                Debug.LogWarning("The Projectile To Spawn Configuration (Projectile Controller) reference in the Spawning Behabiour script is missing.", this);
            return null;
        }
        set { _projectileToSpawnConfig = value; }
    }

    public ProjectilePrefabName ProjectileToSpawn
    {
        get { return _projectilePrefabToSpawn; }
        set { _projectilePrefabToSpawn = value; }
    }

    void Awake()
    {
        if (!_spawnerController)
            _spawnerController = GetComponent<SpawnerController>();

        if (!_projectileToSpawnConfig && _spawnerController.ProjectileName == ProjectilePrefabName.SpawnerConfig)
            _projectileToSpawnConfig = transform.GetChild(0).GetComponent<ProjectileController>();
    }

    void OnEnable() => _hasStartedSpawning = false;

    void Update()
    {
        SetDesactivationVisibility();
        SetActivationVisibility();
        SetTargetting();
        SetSpawning();
    }

    #region Getters&Setters
    public bool IsSpawning {
        get { return _isSpawning; }
        set { _isSpawning = value; }
    }
    public bool StopWhenNotVisible
    {
        get { return _stopWhenNotVisible; }
        set { _stopWhenNotVisible = value; }
    }
    public float TimeBetweenSpawns
    {
        get { return _timeBetweenSpawns; }
        set { _timeBetweenSpawns = value; }
    }
    public int NumberOfProjectiles
    {
        get { return _numberOfProjectilesPerSpawn; }
        set { _numberOfProjectilesPerSpawn = value; }
    }
    public float TimeBeforeActivating
    {
        get { return _timeBeforeActivating; }
        set { _timeBeforeActivating = value; }
    }
    public float SpawningRange
    {
        get { return _spawningRange; }
        set { _spawningRange = value; }
    }
    public bool ActivateWhenVisible
    {
        get { return _activateWhenVisible; }
        set { _activateWhenVisible = value; }
    }
    public bool RandomizePosition
    {
        get { return _randomizePosition; }
        set { _randomizePosition = value; }
    }
    public bool RandomizeDirection
    {
        get { return _randomizeDirection; }
        set { _randomizeDirection = value; }
    }
    public bool RandomizeSpeed
    {
        get { return _randomizeSpeed; }
        set { _randomizeSpeed = value; }
    }
    public bool RandomizeSize
    {
        get { return _randomizeSize; }
        set { _randomizeSize = value; }
    }
    public int NumberOfScopes
    {
        get { return _numberOfScopes; }
        set { _numberOfScopes = value; }
    }
    public float ScopeRange
    {
        set { _scopeRange = value; }
        get { return _scopeRange; }
    }
    public int NumberOfSides
    {
        get { return _numberOfSides; }
        set { _numberOfSides = value; }
    }
    public float SideBending
    {
        get { return _sideBending; }
        set { _sideBending = value; }
    }
    public bool IsTargetting
    {
        get { return _isTargetting; }
        set { _isTargetting = value; }
    }
    public bool SpawningOnRhythm
    {
        get { return _spawningOnRhythm; }
        set { _spawningOnRhythm = value; }
    }
    public float TargettingSpeed
    {
        get { return _targettingSpeed; }
        set { _targettingSpeed = value; }
    }
    public bool SpawnerDirectionAffectsProjectile
    {
        get { return _spawnerDirectionAffectsProjectile; }
        set { _spawnerDirectionAffectsProjectile = value; }
    }

    #endregion

    #region SpecificGetter&Setters
    void SetDesactivationVisibility()
    {
        if (!_stopWhenNotVisible || CameraManager.Instance.IsTargetVisible(gameObject)) return;
        _isSpawning = false;
    }

    void SetActivationVisibility()
    {
        if (!_activateWhenVisible || !CameraManager.Instance.IsTargetVisible(gameObject)) return;
        _isSpawning = true;
    }

    void SetTargetting()
    {
        if (_isTargetting)
        {
            float angle = (_projectileToSpawnConfig.ProjectileMovement.Direction + _projectileToSpawnConfig.ProjectileMovement.CurrentDirection) * Deg2Rad;

            Vector2 direction = _spawnerController.Target.position - transform.position;
            Vector3 forward = new(Cos(angle), Sin(angle));
            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, forward).z;
            _rotationOffset -= rotateAmount * _targettingSpeed;
        }
        else
            _rotationOffset = _spawnerDirectionAffectsProjectile ? _spawnerController.ProjectileMovement.Direction : 0;
    }

    void SetSpawning()
    {
        if (_isSpawning && !_hasStartedSpawning) StartSpawning();
        if(!_isSpawning) StopSpawning();
    }

    //Always actualize spawning boolean when desactivating main visible spawner
    void StartSpawning()
    {
        _hasStartedSpawning = true;

        if (Spawning() != null)
            StopCoroutine(Spawning());

        StartCoroutine(Spawning());
    }

    void StopSpawning()
    {
        _hasStartedSpawning = false;
        if (Spawning() != null)
            StopCoroutine(Spawning());
    }

    float GetSpawningSpeed()
    {
        RecordingPlayer rhythm = _spawnerController.Rhythm;

        if (!rhythm)
            return _timeBetweenSpawns;

        MouseClickingRecorder.Recording recording = rhythm.recordingDictionary[rhythm.recordingTagToPlay];
        if (_spawningOnRhythm && recording.index < recording.timeBetweenClicks.Count - 1)
            return (float)recording.timeBetweenClicks[recording.index];
        else
            return _timeBetweenSpawns;
    }

#endregion
    public IEnumerator Spawning()
    {
        yield return new WaitForSeconds(_timeBeforeActivating);

        while (_isSpawning)
        {
            RadialSpawning();
            float speed = GetSpawningSpeed();
            yield return new WaitForSeconds(speed);
        }
    }

    #region SpawningBehaviourFunctions
    void RadialSpawning()
    {
        ProjectileMovement proj = _projectileToSpawnConfig.ProjectileMovement;
        float angle;
        float initAngle = 0;
        float offSet = 360 / _numberOfScopes;
        float nBProjectilePerScope = _numberOfProjectilesPerSpawn / _numberOfScopes;
        float counter = 0;
        Vector3 projectileSpawningPosition;

        for (int j = 0; j < _numberOfScopes; j++)
        {
            angle = initAngle;

            proj.CurrentSize = GetNewSize(proj.Size, proj.CurrentSize);

            for (int i = 1; i <= nBProjectilePerScope; i++)
            {
                proj.CurrentMoveSpeed = GetNewMoveSpeed(proj.MoveSpeed, counter);
                proj.CurrentDirection = GetNewDirection(proj.Direction, angle, offSet);

                if (counter < _numberOfProjectilesPerSpawn)
                    counter++;

                projectileSpawningPosition = GetNewPosition(angle);

                _spawnerController.ObjectPooler.SpawnFromPool(_projectilePrefabToSpawn, _projectileToSpawnConfig, projectileSpawningPosition);
                angle += offSet*_scopeRange/ nBProjectilePerScope;
            }
            initAngle += offSet;
        }
    }

    float GetNewDirection(float dir, float angle,  float offSet)
    {
        return _randomizeDirection ?
            (dir + (angle + Random.Range(0, offSet * _scopeRange)) * _spawningRange + _rotationOffset) :
            (dir + angle * _spawningRange + _rotationOffset);
    }

    float GetNewSize(float newSize, float currentSize)
    {
        return _randomizeSize ?
                Random.Range(1, 100 * newSize) / 100f :
                currentSize;
    }

    float GetNewMoveSpeed(float speed, float step)
    {
        return _randomizeSpeed ?
            (Random.Range(10, 100 * speed) / 100f) :
            (speed + Pow(Sin(_numberOfSides * PI / _numberOfProjectilesPerSpawn * step), 2) * _sideBending);
    }

    Vector3 GetNewPosition(float angle)
    {
        if (_randomizePosition)
        {
            float teta = Deg2Rad * _scopeRange * 360;
            float radius = Random.Range(0, _spawningRange * CameraManager.Instance.GetScreenBounds().x);
            float randAngle = Random.Range(angle, angle + teta);
            float x = transform.position.x + radius * Cos(randAngle);
            float y = transform.position.y + radius * Sin(randAngle);
            Vector3 randomPosition = new(x, y);
            return randomPosition;
        }
        else
            return transform.position;
    }
    #endregion
}
