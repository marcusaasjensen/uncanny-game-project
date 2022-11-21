using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(ProjectileMovement))]
public abstract class ProjectileController: MonoBehaviour, IPooledObject<ProjectileController>
{
    [Tooltip("Name of the projectile. Must be assigned for each prefab but assignment not relevant for the template projectiles (TMP).")]
    [SerializeField] protected ProjectilePrefabName projectileName;
    [SerializeField] ProjectileMovement _projectileMovement;
    [SerializeField] protected Transform target;
    [SerializeField] RecordingPlayer _rhythm;


    [Header("Optional")]
    [SerializeField] ProjectileCollision _projectileCollision;
    [SerializeField] ProjectileAnimation _projectileAnimation;

    [Header("Life-time")]
    [Tooltip("If is immortal, the projectile will never self despawn.")]
    [SerializeField] protected bool isImmortal;
    [Tooltip("Time before the projectile despawns.")]
    [SerializeField] [Min(0)] protected float timeToLive;
    [SerializeField] protected bool disappearWhenTouchingTarget;
    [Tooltip("The projectile self-despawn when being outside of camera visibility.")]
    [SerializeField] protected bool despawnWhenNotVisible;
    
    [Header("Damage")]
    [SerializeField] protected int damage;
    [SerializeField] [Min(0)] protected float minimumSizeOfDamage;

    [Header("Configuration")]
    [Tooltip("The projectile can be affected in real-time with the configuration instead of being affected only when spawning.")]
    [SerializeField] protected bool realTimeConfiguration;

    IEnumerator _selfDespawn;

    public ProjectileMovement ProjectileMovement
    {
        get { return _projectileMovement; }
        set { _projectileMovement = value; }
    }

    public ProjectileCollision ProjectileCollision
    {
        get { return _projectileCollision; }
        set { _projectileCollision = value; }
    }

    public ProjectileAnimation ProjectileAnimation
    {
        get { return _projectileAnimation; }
        set { _projectileAnimation = value; }
    }

    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }

    public RecordingPlayer Rhythm
    {
        get { return _rhythm; }
        set { _rhythm = value; }
    }

    public ProjectilePrefabName Name
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

    public bool IsImmortal
    {
        get { return isImmortal; }
        set { isImmortal = value; }
    }

    public bool DisappearWhenTouchingTarget
    {
        get { return disappearWhenTouchingTarget; }
        set { disappearWhenTouchingTarget = value; }
    }

    public bool RealTimeConfiguration
    {
        get { return realTimeConfiguration; }
        set { realTimeConfiguration = value; }
    }

    public void OnObjectSpawn(ProjectileController projectile){
        SetInitialValues(projectile); //values that will change only when spawning
        SetProjectile(projectile);
        StartLifeTime();
    }


    void SetInitialValues(ProjectileController projectile)
    {
        if(_projectileMovement == null)
        {
            Debug.LogWarning("Projectile Movement reference in projectile controller is missing.");
            return;
        }
        _projectileMovement.CurrentSize = projectile._projectileMovement.CurrentSize;
        _projectileMovement.CurrentDirection = projectile._projectileMovement.CurrentDirection;
        _projectileMovement.CurrentMoveSpeed = projectile._projectileMovement.CurrentMoveSpeed;
        _projectileMovement.AppearanceTime = Time.time;
    }

    public void SetProjectile(ProjectileController projectile)
    {
        damage = projectile.damage;
        timeToLive = projectile.timeToLive;
        isImmortal = projectile.isImmortal;
        disappearWhenTouchingTarget = projectile.disappearWhenTouchingTarget;
        despawnWhenNotVisible = projectile.despawnWhenNotVisible;
        realTimeConfiguration = projectile.realTimeConfiguration;
        minimumSizeOfDamage = projectile.minimumSizeOfDamage;
        target = projectile.target;
        _rhythm = projectile.Rhythm;

        _projectileMovement.SetProjectileMovement(projectile._projectileMovement);
    }

    void FixedUpdate(){
        SetVisibility();
        SetDamageAccordingToSize();
    }

    void SetVisibility()
    {
        if (!despawnWhenNotVisible || CameraManager.Instance.IsTargetVisible(gameObject)) return;
        this.gameObject.SetActive(false);
    }

    int tmpDamage;

    void SetDamageAccordingToSize()
    {
        if (Mathf.Abs(_projectileMovement.Size) < minimumSizeOfDamage)
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
        if(_projectileAnimation != null)
            _projectileAnimation.StartAnimation(); //think for desactivating damage at the start animation or not
        if (isImmortal) return; // make a boolean variable to check if projectile is immortal or not
        if (_selfDespawn != null) { StopCoroutine(_selfDespawn); }
        _selfDespawn = SelfDespawnAfter(timeToLive);
        StartCoroutine(_selfDespawn);
    }

    IEnumerator SelfDespawnAfter(float ttl)
    {
        if (_projectileCollision != null)
        {
            _projectileCollision.EnableCollider(true);
            yield return new WaitForSeconds(ttl);
            _projectileCollision.EnableCollider(false);
        }
        else
            yield return new WaitForSeconds(ttl);

        yield return new WaitForSeconds(_projectileAnimation != null ? _projectileAnimation.EndAnimation() : 0);
        this.gameObject.SetActive(false);
    }
}
