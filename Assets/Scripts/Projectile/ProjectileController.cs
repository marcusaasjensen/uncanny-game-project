using System.Collections;
using UnityEngine;

public enum ProjectileName
{
    Bullet,
    Red_Bullet,
    Square_Bullet,
    Spawner,
    SpawnerTMP,
    BulletTMP,

}

[DisallowMultipleComponent]
[RequireComponent(typeof(ProjectileMovement))]
public abstract class ProjectileController: MonoBehaviour, IPooledObject<ProjectileController>
{
    public ProjectileMovement projectileMovement;
    public ProjectileCollision projectileCollision;
    public ProjectileAnimation projectileAnimation;
    public Transform target;

    [Tooltip("Name of the projectile. Must be assigned for each prefab but assignment not relevant for the template projectiles (TMP).")]
    [SerializeField] protected ProjectileName projectileName;

    [Header("To player")]
    [SerializeField] protected int damage;
    [SerializeField] protected float minimumSizeOfDamage;

    [Header("Options")]

    [Tooltip("Time before the projectile despawning. If negative, the projectile will never despawn.")]
    [SerializeField] protected float timeToLive;

    [SerializeField] protected bool disappearWhenTouchingTarget;

    [Tooltip("The projectile self-despawn when being outside of camera visibility.")]
    [SerializeField] protected bool visibilityDespawn;

    [Tooltip("The projectile can be affected in run-time instead of being affected only when spawning.")]
    [SerializeField] protected bool isContinuouslyAffected;

    CameraManager _cameraManager;
    IEnumerator _selfDespawn;

    public ProjectileName Name
    {
        get { return projectileName; }
        set { projectileName = value; }
    }

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }
    public float MinimumSizeOfDamage
    {
        get { return minimumSizeOfDamage; }
        set { minimumSizeOfDamage = value; }
    }
    public float TimeToLive
    {
        get { return timeToLive; }
        set { timeToLive = value; }
    }

    public bool DisappearWhenTouchingTarget
    {
        get { return disappearWhenTouchingTarget; }
        set { disappearWhenTouchingTarget = value; }
    }

    public bool IsContinuouslyAffected
    {
        get { return isContinuouslyAffected; }
        set { isContinuouslyAffected = value; }
    }
    public CameraManager CameraManager
    {
        get { return _cameraManager; }
        set { if (_cameraManager != value) { _cameraManager = value; } }
    }

    public void OnObjectSpawn(ProjectileController projectile){
        SetInitialValues(projectile); //values that will change only when spawning
        SetProjectile(projectile);
        StartLifeTime();
    }


    void SetInitialValues(ProjectileController projectile)
    {
        projectileMovement.CurrentSize = projectile.projectileMovement.CurrentSize;
        projectileMovement.CurrentDirection = projectile.projectileMovement.CurrentDirection;
        projectileMovement.CurrentMoveSpeed = projectile.projectileMovement.CurrentMoveSpeed;
        projectileMovement.AppearanceTime = Time.time;
    }

    public void SetProjectile(ProjectileController projectile)
    {
        damage = projectile.damage;
        timeToLive = projectile.timeToLive;
        disappearWhenTouchingTarget = projectile.disappearWhenTouchingTarget;
        visibilityDespawn = projectile.visibilityDespawn;
        isContinuouslyAffected = projectile.isContinuouslyAffected;
        minimumSizeOfDamage = projectile.minimumSizeOfDamage;
        target = projectile.target;

        projectileMovement.SetProjectileMovement(projectile.projectileMovement);
    }

    void Awake()
    {
        _cameraManager = CameraManager.Instance;
    }

    void FixedUpdate(){
        SetVisibility();
        SetDamageAccordingToSize();
    }

    void SetVisibility()
    {
        if (!visibilityDespawn || _cameraManager.IsTargetVisible(gameObject)) return;
        this.gameObject.SetActive(false);
    }

    int tmpDamage;

    void SetDamageAccordingToSize()
    {
        if (Mathf.Abs(projectileMovement.Size) < minimumSizeOfDamage)
        {
            tmpDamage = damage;
            damage = 0;
        }
        else
            if(damage == 0)
               damage = tmpDamage;
    }

    void StartLifeTime()
    {
        if(projectileAnimation != null)
            projectileAnimation.StartAnimation(); //think for desactivating damage at the start animation or not
        if (timeToLive < 0) return; // make a boolean variable to check if projectile is immortal or not
        if (_selfDespawn != null) { StopCoroutine(_selfDespawn); }
        _selfDespawn = SelfDespawnAfter(timeToLive);
        StartCoroutine(_selfDespawn);
    }

    IEnumerator SelfDespawnAfter(float ttl)
    {
        if (projectileCollision != null)
        {
            projectileCollision.EnableCollider(true);
            yield return new WaitForSeconds(ttl);
            projectileCollision.EnableCollider(false);
        }
        else
            yield return new WaitForSeconds(ttl);

        yield return new WaitForSeconds(projectileAnimation != null ? projectileAnimation.EndAnimation() : 0);
        this.gameObject.SetActive(false);
    }
}
