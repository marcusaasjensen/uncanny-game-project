using UnityEngine;

public enum PatternName
{
    Default,
    Tentacular,
    Weed,
}

[DisallowMultipleComponent]
[RequireComponent(typeof(SpawnerController))]
public class SpawnerPatternController : MonoBehaviour
{
    [SerializeField] SpawnerController _spawner;

    [SerializeField] PatternName _patternName = default;

    PatternName _patternNameBuffer = default;

    void FixedUpdate()
    {
        SetPatternName();
    }

    void SetPatternName()
    {
        if (_patternName == _patternNameBuffer) return;

        _patternNameBuffer = _patternName;
        switch (_patternName)
        {
            case PatternName.Default:
                break;
            case PatternName.Tentacular:
                Tentacular();
                break;
            case PatternName.Weed:
                Weed();
                break;
            default:
                break;
        }
    }

    void SetSpawnerPattern(
        //Spawner
        float spawningSpeed, 
        float spawningRange, 
        int nbProj,
        MovementBehaviour spawnerMB, 
        float spawnerSpeed,
        bool isSpawnerContinous, 
        //Projectile
        float bulletSpeed, 
        float bulletDir, 
        float ttlBullet, 
        MovementBehaviour bulletMB, 
        bool isBulletContinous
    )
    {
        SpawnerController spawnerController = _spawner.GetComponent<SpawnerController>();
        ProjectileController projectileController = _spawner.SpawningBehaviour.ProjectileToSpawnConfig.GetComponent<ProjectileController>();

        //Default settings
        spawnerController.SpawningBehaviour.IsSpawning = true;

        //Setting
        spawnerController.SpawningBehaviour.TimeBetweenSpawns = spawningSpeed;
        spawnerController.SpawningBehaviour.NumberOfProjectiles = nbProj;
        spawnerController.SpawningBehaviour.SpawningRange = spawningRange;
        spawnerController.SpawningBehaviour.SpawningRange = spawningRange;
        spawnerController.SpawningBehaviour.SpawningRange = spawningRange;

        spawnerController.ProjectileMovement.MovementBehaviour = spawnerMB;
        spawnerController.ProjectileMovement.MoveSpeed = spawnerSpeed;

        spawnerController.RealTimeConfiguration = isSpawnerContinous;

        projectileController.ProjectileMovement.MoveSpeed = bulletSpeed;
        projectileController.ProjectileMovement.MovementBehaviour = bulletMB;
        projectileController.ProjectileMovement.Direction = bulletDir;
        projectileController.TimeToLive = ttlBullet;

        projectileController.RealTimeConfiguration = isBulletContinous;

        _spawner.SpawningBehaviour.ProjectileToSpawnConfig = projectileController;

        _spawner.SetProjectile(spawnerController);
    }

    void Tentacular()
    {
        SetSpawnerPattern(
            0.1f, 
            1, 
            8, 
            MovementBehaviour.Static, 
            _spawner.ProjectileMovement.MoveSpeed, 
            _spawner.RealTimeConfiguration, 
            5.0f, 
            30.0f, 
            1.0f, 
            MovementBehaviour.AllCircular, 
            _spawner.SpawningBehaviour.ProjectileToSpawnConfig.RealTimeConfiguration
        );
    }

    void Weed()
    {
        SetSpawnerPattern(
            0.22f, 
            1, 
            7, 
            MovementBehaviour.SimpleCircular, 
            1.6f, 
            _spawner.RealTimeConfiguration, 
            1.3f, 
            0, 
            6.0f, 
            MovementBehaviour.SimpleCircular, 
            _spawner.SpawningBehaviour.ProjectileToSpawnConfig.RealTimeConfiguration
        );
     }

}