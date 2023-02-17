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

    IEnumerator _lifeTime;

    public ProjectileMovement ProjectileMovement
    {
        get 
        {
            if (_projectileMovement)
                return _projectileMovement;
            else
                Debug.LogWarning("Projectile Movement reference in Projectile Controller script is missing.", this);
            return null;
        }
        set { _projectileMovement = value; }
    }

    public ProjectileCollision ProjectileCollision
    {
        get
        {
            if (_projectileCollision)
                return _projectileCollision;
            else
                Debug.LogWarning("Projectile Collision reference in Projectile Controller script is missing.", this);
            return null;
        }
        set { _projectileCollision = value; }
    }

    public ProjectileAnimation ProjectileAnimation
    {
        get
        {
            if (_projectileAnimation)
                return _projectileAnimation;
            else
                Debug.LogWarning("Projectile Animation reference in Projectile Controller script is missing.", this);
            return null;
        }
        set { _projectileAnimation = value; }
    }

    public Transform Target
    {
        get
        {
            if (target)
                return target;
            else
                Debug.LogWarning("Target (Transform) reference in Projectile Controller script is missing.", this);
            return null;
        }
        set { target = value; }
    }

    public RecordingPlayer Rhythm
    {
        get
        {
            if (_rhythm)
                return _rhythm;
            else
                Debug.LogWarning("Rhythm (Recording Player) reference in Projectile Controller script is missing.", this);
            return null;
        }
        set { _rhythm = value; }
    }

    public ProjectilePrefabName ProjectileName
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

        if (_lifeTime != null)
            StopCoroutine(_lifeTime);

        _lifeTime = StartLifeTime();
        StartCoroutine(_lifeTime);
    }

    void SetInitialValues(ProjectileController projectile)
    {
        if(!_projectileMovement)
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

    void FixedUpdate()
    {
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

    IEnumerator StartLifeTime()
    {
        //Start life time with start animation
        EnableProjectileCollision(false);

        if (_projectileAnimation)
            _projectileAnimation.StartAnimation();

        yield return new WaitForSeconds(_projectileAnimation.AnimationLengthOffset);
        //enable collider during life time
        EnableProjectileCollision(true);
        
        //Doesn't continue lifetime if immortal
        while (isImmortal);
        yield return new WaitForSeconds(timeToLive);
        EnableProjectileCollision(false);


        //End life time with ending animation.
        if (_projectileAnimation)
            _projectileAnimation.EndAnimation();

        yield return new WaitForSeconds(_projectileAnimation.AnimationLengthOffset);

        this.gameObject.SetActive(false);
    }

    void EnableProjectileCollision(bool val)
    {
        if (_projectileCollision) _projectileCollision.EnableCollider(val);
    }
}
