using UnityEngine;
using static UnityEngine.Mathf;

public enum MovementBehaviour
{
    Unchanged,
    Static,
    Regular,
    Sinusoidal,
    ZigZag,
    Circular,
    SimpleCircular,
    AllCircular,
    Pivot,
    AllPivot,
    Clock,
    Looping,
    Wavy,
    Knot,
    Particle,
    FollowTarget,
    MissileTarget,
}

[DisallowMultipleComponent]
public class ProjectileMovement : MonoBehaviour
{
    public ProjectileController projectileController;
    public SettingBehaviour settingBehaviour;

    [Header("Movement")]
    [SerializeField] float _moveSpeed;
    [SerializeField] float _direction;
    [SerializeField] [Range(0,360)] float _rotation;
    [SerializeField] MovementBehaviour _movementBehaviour = default;

    [Header("Size")]
    [SerializeField] float _size;

    [Header("Options")]
    [SerializeField] bool _rotateToDirection;

    float _appearanceTime;
    
    //Values that changes constantly (ony serialized fields can be changed as settings)
    float _currentDirection;
    float _currentSize;
    float _currentMoveSpeed;
    Vector3 _vectorMovement;

    const int _FPS = 60;

    Vector3 _initialLocalScale;
    Transform _transform;

    void Awake()
    {
        SetTransform();
    }

    void FixedUpdate()
    {
        SetRotation();
        SetLocalScale();
        SetMovementBehaviour();
    }

    #region Getters&Setters
    public float Direction
    {
        get { return _direction; }
        set
        {
            if (value > 360)
                _direction = value - 360;
            else if (value < -360)
                _direction = value + 360;
            else
                _direction = value;
        }
    }
    public float Rotation
    {
        get { return _rotation; }
        set
        {
            if (value > 360)
                _rotation = value - 360;
            else if (value < -360)
                _rotation = value + 360;
            else
                _rotation = value;
        }
    }
    public float Size
    {
        get { return _size; }
        set { _size = value; }
    }
    public float CurrentSize
    {
        get { return _currentSize; }
        set { _currentSize = value; }
    }
    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }
    public float CurrentMoveSpeed
    {
        set { _currentMoveSpeed = value; }
        get { return _currentMoveSpeed; }
    }
    public MovementBehaviour MovementBehaviour
    {
        get { return _movementBehaviour; }
        set { _movementBehaviour = value; }
    }
    public float AppearanceTime
    {
        get { return _appearanceTime; }
        set { _appearanceTime = value; }
    }
    public float CurrentDirection
    {
        get { return _currentDirection; }
        set { _currentDirection = value; }
    }
    public bool RotateToDirection
    {
        get { return _rotateToDirection; }
        set { _rotateToDirection = value; }
    }

    #endregion
    #region SpecificGetters&Setters
    void SetTransform()
    {
        _transform = transform;
        _initialLocalScale = _transform.localScale;
    }

    void SetRotation()
    {
        if (_rotateToDirection)
        {
            Vector2 dir = _vectorMovement;
            float angle = Atan2(dir.y, dir.x) * Rad2Deg;
            _transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
            _transform.rotation = Quaternion.AngleAxis(_rotation, Vector3.forward);
    }

    void SetLocalScale()
    {
        float scale = _size + _currentSize;
        _transform.localScale = _initialLocalScale;
        _transform.localScale *= scale;
    }    

    public void SetProjectileMovement(ProjectileMovement tmp)
    {
        _size = tmp.Size;
        _moveSpeed = tmp.MoveSpeed;
        _rotation = tmp.Rotation;
        _direction = tmp.Direction;
        _movementBehaviour = tmp.MovementBehaviour;
        _rotateToDirection = tmp.RotateToDirection;
        settingBehaviour?.SetSpeedBehaviour(tmp.settingBehaviour);
    }

    void SetMovementBehaviour()
    {
        //Vector3 validVectorMovement = float.IsNaN(_vectorMovement.x + _vectorMovement.y) ? Vector3.one : _vectorMovement;
        _transform.Translate(_vectorMovement, relativeTo:Space.World);
        switch (_movementBehaviour)
        {
            case MovementBehaviour.Unchanged:
                break;
            case MovementBehaviour.Static:
                StaticBehaviour();
                break;
            case MovementBehaviour.Regular:
                RegularBehaviour();
                break;
            case MovementBehaviour.Sinusoidal:
                SinusoidalBehaviour();
                break;
            case MovementBehaviour.Circular:
                CircularBehaviour();
                break;
            case MovementBehaviour.SimpleCircular:
                SimpleCircularBehaviour();
                break;
            case MovementBehaviour.AllCircular:
                AllCircularBehaviour();
                break;
            case MovementBehaviour.Wavy:
                WavyBehaviour();
                break;
            case MovementBehaviour.Looping:
                LoopingBehaviour();
                break;
            case MovementBehaviour.Clock:
                ClockBehaviour();
                break;
            case MovementBehaviour.Knot:
                KnotBehaviour();
                break;
            case MovementBehaviour.Pivot:
                PivotBehaviour();
                break;
            case MovementBehaviour.AllPivot:
                AllPivotBehaviour();
                break;
            case MovementBehaviour.Particle:
                ParticleBehaviour();
                break;
            case MovementBehaviour.FollowTarget:
                FollowTargetBehaviour();
                break;
            case MovementBehaviour.MissileTarget:
                MissileTargetBehaviour();
                break;
            case MovementBehaviour.ZigZag:
                ZigzagBehaviour();
                break;

            default:
                break;
        }
    }
    #endregion
    #region MovementBehaviourFunctions

    void SinusoidalBehaviour()
    {
        float angle = (_direction + _currentDirection) * Deg2Rad;
        float speed = (_moveSpeed + _currentMoveSpeed) * Time.deltaTime;
        float dt = Time.time - _appearanceTime;

        Vector3 forward = new(Cos(angle), Sin(angle));

        Vector3 sinNormal = new(Cos(Cos(4 * PI * dt) + angle), Sin(Sin( 4 * PI * dt) + angle),0);

        Vector3 movement = forward + sinNormal;

        _vectorMovement = movement * speed;
    }

    void ZigzagBehaviour()
    {
        float angle = (_direction + _currentDirection) * Deg2Rad;
        float speed = (_moveSpeed + _currentMoveSpeed) * Time.deltaTime;
        float dt = Time.time - _appearanceTime;

        Vector3 sinNormal = new(Cos(Cos(4 * PI * dt) + angle), Sin(Sin(4 * PI * dt) + angle), 0);

        Vector3 movement = sinNormal;

        _vectorMovement = movement * speed;
    }

    void MissileTargetBehaviour()
    {
        float angle = (_direction + _currentDirection) * Deg2Rad;
        float speed = (_moveSpeed + _currentMoveSpeed) * Time.deltaTime;

        Vector2 direction = projectileController.target.position - _transform.position;
        Vector3 forward = new(Cos(angle), Sin(angle));
        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, forward).z;
        _currentDirection -= rotateAmount * _direction;
        _vectorMovement = forward * speed;
    }

    void ParticleBehaviour()
    {
        float randomAngle = Random.Range(-10, 10);
        RegularBehaviour();
        _currentDirection += randomAngle;
    }

    void AllPivotBehaviour()
    {
        float t = Time.time;
        float angle = (_direction + _currentDirection) * Deg2Rad;
        float speed = (_moveSpeed + _currentMoveSpeed) * Time.deltaTime;
        _vectorMovement = new Vector3(Cos(angle + t), Sin(angle + t)) * speed;
    }    
    
    void PivotBehaviour()
    {
        float t1 = _appearanceTime;
        float angle = (_direction + _currentDirection) * Deg2Rad;
        float speed = (_moveSpeed + _currentMoveSpeed) * Time.deltaTime;
        _vectorMovement = new Vector3(Cos(angle + t1), Sin(angle + t1)) * speed;
    }


    void StaticBehaviour()
    {
        _vectorMovement = Vector3.zero;
    }

    void RegularBehaviour()
    {
        float angle = (_direction + _currentDirection) * Deg2Rad;
        float speed = (_moveSpeed + _currentMoveSpeed) * Time.deltaTime;

        Vector3 dir = new (Cos(angle), Sin(angle));
        _vectorMovement = dir * speed;
    }

    void KnotBehaviour()
    {
        float t1 = _appearanceTime;
        float t = Time.time;

        RegularBehaviour();
        _vectorMovement.x *= Cos(t1+t);
        _vectorMovement.y *= Sin(t1+t);
    }

    void CircularBehaviour()
    {
        float t1 = _appearanceTime;

        RegularBehaviour();
        _vectorMovement.x *= Cos(t1);
        _vectorMovement.y *= Sin(t1);
    }

    void AllCircularBehaviour()
    {
        float t = Time.time;

        RegularBehaviour();
        _vectorMovement.x *= Cos(t);
        _vectorMovement.y *= Sin(t);
    }


    void WavyBehaviour()
    {
        float angle = (_direction + _currentDirection) * Deg2Rad;
        float speed = (_moveSpeed + _currentMoveSpeed);
        float t1 = _appearanceTime;

        _vectorMovement = new Vector3(Cos(angle), Sin(angle)) * speed * Time.deltaTime;

        _currentDirection += Cos(t1 * speed); //cadence of the waviness
        _currentDirection -= Sin(t1 * speed);
    }

    void LoopingBehaviour()
    {
        float angle = (_direction + _currentDirection) * Deg2Rad;
        float speed = (_moveSpeed + _currentMoveSpeed) * Time.deltaTime;
        float dt = Time.time - _appearanceTime;
        float offSet = 0.25f;

        //derivative of this function: https://math.stackexchange.com/questions/276933/graph-of-an-infinitely-extending-rollercoaster-loop
        _vectorMovement = new Vector3(-Sin(dt) + Cos(angle) * offSet, Cos(dt) - Sin(angle) * offSet) * speed;
    }

    void ClockBehaviour()
    {
        float angle = (_direction + _currentDirection) * Deg2Rad;
        float speed = (_moveSpeed + _currentMoveSpeed) * Time.deltaTime;
        float t1 = _appearanceTime;

        _vectorMovement = new Vector3(Pow(Cos(angle * t1), 3), Pow(Sin(angle * t1), 3)) * speed;
    }

    void SimpleCircularBehaviour()
    {
        RegularBehaviour();
        _currentDirection++;
    }

    void FollowTargetBehaviour()
    {
        Transform trg = projectileController.target;
        if (trg == null)
        {
            Debug.LogWarning("the projectile can't follow the target because the target is not referenced in the ProjectileMovement component.");
            return;
        }

        float distanceToTarget = Abs(Vector3.Distance(trg.position, _transform.position));

        if (!trg.gameObject.activeSelf || distanceToTarget <= 0.5f) return;

        float speed = (_moveSpeed + _currentMoveSpeed) * Time.deltaTime;
        Vector3 followPosition = trg.position - _transform.position;

        _vectorMovement = followPosition * speed;
    }
#endregion
}
