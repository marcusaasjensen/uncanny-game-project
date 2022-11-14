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
    public SpawnerController spawner;

    [SerializeField] PatternName _patternName = default;

    PatternName _patternNameTMP = default;

    void FixedUpdate()
    {
        SetPatternName();
    }

    void SetPatternName()
    {
        if (_patternName == _patternNameTMP) return;

        _patternNameTMP = _patternName;
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
        SpawnerController spawnerController = spawner.GetComponent<SpawnerController>();
        ProjectileController projectileController = spawner.spawningBehaviour.projectileToSpawnConfig.GetComponent<ProjectileController>();

        //Default settings
        spawnerController.spawningBehaviour.IsSpawning = true;

        //Setting
        spawnerController.spawningBehaviour.TimeBetweenSpawns = spawningSpeed;
        spawnerController.spawningBehaviour.NumberOfProjectiles = nbProj;
        spawnerController.spawningBehaviour.SpawningRange = spawningRange;
        spawnerController.spawningBehaviour.SpawningRange = spawningRange;
        spawnerController.spawningBehaviour.SpawningRange = spawningRange;

        spawnerController.projectileMovement.MovementBehaviour = spawnerMB;
        spawnerController.projectileMovement.MoveSpeed = spawnerSpeed;

        spawnerController.IsContinuouslyAffected = isSpawnerContinous;

        projectileController.projectileMovement.MoveSpeed = bulletSpeed;
        projectileController.projectileMovement.MovementBehaviour = bulletMB;
        projectileController.projectileMovement.Direction = bulletDir;
        projectileController.TimeToLive = ttlBullet;

        projectileController.IsContinuouslyAffected = isBulletContinous;

        spawner.spawningBehaviour.projectileToSpawnConfig = projectileController;

        spawner.SetProjectile(spawnerController);
    }

    void Tentacular()
    {
        SetSpawnerPattern(
            0.1f, 
            1, 
            8, 
            MovementBehaviour.Static, 
            spawner.projectileMovement.MoveSpeed, 
            spawner.IsContinuouslyAffected, 
            5.0f, 
            30.0f, 
            1.0f, 
            MovementBehaviour.AllCircular, 
            spawner.spawningBehaviour.projectileToSpawnConfig.IsContinuouslyAffected
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
            spawner.IsContinuouslyAffected, 
            1.3f, 
            0, 
            6.0f, 
            MovementBehaviour.SimpleCircular, 
            spawner.spawningBehaviour.projectileToSpawnConfig.IsContinuouslyAffected
        );
     }

}