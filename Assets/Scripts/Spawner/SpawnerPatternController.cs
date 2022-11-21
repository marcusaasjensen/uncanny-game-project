using UnityEngine;

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
            spawningSpeed: 0.1f, 
            spawningRange: 1, 
            nbProj: 8, 
            spawnerMB: MovementBehaviour.Static, 
            spawnerSpeed: _spawner.ProjectileMovement.MoveSpeed, 
            isSpawnerContinous: _spawner.RealTimeConfiguration, 
            bulletSpeed: 5.0f, 
            bulletDir: 30.0f, 
            ttlBullet: 1.0f, 
            bulletMB: MovementBehaviour.AllCircular, 
            isBulletContinous: _spawner.SpawningBehaviour.ProjectileToSpawnConfig.RealTimeConfiguration
        );
    }

    void Weed()
    {
        SetSpawnerPattern(
            spawningSpeed: 0.22f,
            spawningRange: 1,
            nbProj: 7,
            spawnerMB: MovementBehaviour.SimpleCircular,
            spawnerSpeed: 1.6f,
            isSpawnerContinous: _spawner.RealTimeConfiguration,
            bulletSpeed: 1.3f,
            bulletDir: 0,
            ttlBullet: 6.0f,
            bulletMB: MovementBehaviour.SimpleCircular,
            isBulletContinous: _spawner.SpawningBehaviour.ProjectileToSpawnConfig.RealTimeConfiguration
        ); ;
     }

}