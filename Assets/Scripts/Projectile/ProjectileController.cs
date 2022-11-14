using System.Collections;
using System.Collections.Generic;
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
    public ProjectileAnimation projectileAnimation;
    public Collider2D projectileCollider;
    public List<AudioClip> collisionSounds;

    [Tooltip("Name of the projectile. Must be assigned for each prefab but assignment not relevant for the template projectiles (TMP).")]
    [SerializeField] protected ProjectileName projectileName;

    [Header("To player")]
    [SerializeField] protected int damage;
    [SerializeField] protected float minimumSizeOfDamage;

    [Header("Options")]

    [Tooltip("Time before the projectile despawning. If negative, the projectile will never despawn.")]
    [SerializeField] protected float timeToLive;

    [Tooltip("The projectile self-despawn when being outside of camera visibility.")]
    [SerializeField] protected bool visibilityDespawn;

    [Tooltip("The projectile can be affected in run-time instead of being affected only when spawning.")]
    [SerializeField] protected bool isContinuouslyAffected;

    [SerializeField] protected bool disappearWhenTouchingPlayer;

    CameraManager _cameraManager;
    IEnumerator _selfDespawn;
    bool _hasCollided = false;

    public ProjectileName Name
    {
        get { return projectileName; }
        set { projectileName = value; }
    }
    public bool DisappearWhenTouchingPlayer
    {
        get { return disappearWhenTouchingPlayer; }
        set { disappearWhenTouchingPlayer = value; }
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

    public bool HasCollided
    {
        get { return _hasCollided; }
        set { _hasCollided = value; }
    }

    public void OnObjectSpawn(ProjectileController projectile){
        SetInitialValues(projectile); //values that will change only when spawning
        SetProjectile(projectile);
        StartLifeTime();
    }

    void StartLifeTime()
    {
        if(projectileAnimation != null)
            projectileAnimation.StartAnimation();
        if (timeToLive < 0) return;
        if (_selfDespawn != null) { StopCoroutine(_selfDespawn); }
        _selfDespawn = SelfDespawnAfter(timeToLive);
        StartCoroutine(_selfDespawn);
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
        disappearWhenTouchingPlayer = projectile.disappearWhenTouchingPlayer;
        visibilityDespawn = projectile.visibilityDespawn;
        isContinuouslyAffected = projectile.isContinuouslyAffected;
        minimumSizeOfDamage = projectile.minimumSizeOfDamage;
        projectileMovement.SetProjectileMovement(projectile.projectileMovement);
    }

    void Awake()
    {
        projectileCollider = GetComponent<Collider2D>();
        _cameraManager = CameraManager.Instance;
    }

    void FixedUpdate(){
        SetVisibility();
        SetDamageAccordingToSize();
        SetCollision();
    }

    void SetVisibility()
    {
        if (!visibilityDespawn || _cameraManager.IsTargetVisible(gameObject)) return;
        this.gameObject.SetActive(false);
    }

    void SetCollision()
    {
        if (_hasCollided)
        {
            SoundManager.Instance.PlayRandomSound(collisionSounds, true);
            _hasCollided = false;
            if (disappearWhenTouchingPlayer)
                gameObject.SetActive(false);
        }
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
            if(damage==0)
               damage = tmpDamage;
    }

    IEnumerator SelfDespawnAfter(float ttl)
    {
        EnableCollider(true);
        yield return new WaitForSeconds(ttl);
        EnableCollider(false);

        yield return new WaitForSeconds(projectileAnimation!=null?projectileAnimation.EndAnimation():0);
        this.gameObject.SetActive(false);
    }

    void EnableCollider(bool value)
    {
        if (projectileCollider == null) return;
        projectileCollider.enabled = value; 
    }
}
