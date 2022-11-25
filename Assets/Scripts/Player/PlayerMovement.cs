using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class PlayerMovement : MonoBehaviour
{
    PlayerInputActions playerActions;

    [SerializeField] PlayerController _playerController;

    [SerializeField] float _moveSpeed;
    [SerializeField] [Min(0)] float _startingMoveDuration;
    [SerializeField] AnimationCurve _startingMoveCurve;
    [SerializeField] [Min(0)] float _endingMoveDuration;
    [SerializeField] AnimationCurve _endingMoveCurve;
    [SerializeField] [Range(0,1)] float _sneakingAmount;
    [SerializeField] [Min(0)] float _speedTransitionDuration;
    [SerializeField] ParticleSystem _dashTrailParticle;
    [SerializeField] ParticleSystem _startDashParticle;
    [SerializeField] List<AudioClip> _dashSounds;
    [SerializeField] float _dashSpeed;
    [SerializeField] float _dashDuration;
    [SerializeField] [Min(0)] float _timeBeforeNextDash;
    [SerializeField] float _minimumSize;
    [SerializeField] bool _changeScale;
    [SerializeField] bool _isRotatingToDirection = true;
    [SerializeField] float _timeToRotate = 0;

    InputAction _moveInput;
    InputAction _sneakInput;
    InputAction _dashInput;

    Vector2 _inputDirection = Vector2.zero;
    Vector2 _currentDirection = Vector2.zero;
    Vector2 _lastDirection = Vector2.zero;
    Vector2 _defaultScale;

    float _currentMoveSpeed;
    float _currentDashSpeed;
    float _currentSpeedTransition;
    float _currentDashDuration;
    float _currentCurveTime;
    float _currentEndingMoveDuration;
    float _currentTimeToRotate;
    float _currentTimeBeforeNextDash;

    bool _isMoving = false;
    bool _isDashing = false;

    public bool IsDashing
    {
        get { return _isDashing; }
    }

    Transform _transform;

    Transform _cam;

    void Awake()
    {

        if(!_playerController)
            _playerController = GetComponent<PlayerController>();

        _cam = Camera.main.transform;
        _transform = GetComponent<Transform>();
        _defaultScale = _transform.localScale;

        playerActions = new PlayerInputActions();

        _currentMoveSpeed = _moveSpeed;
        _currentDashSpeed = 0;
        _currentTimeToRotate = 0;
        _currentDashDuration = _dashDuration;
        _currentSpeedTransition = _speedTransitionDuration;
        _currentEndingMoveDuration = _endingMoveDuration;
        _currentTimeBeforeNextDash = _timeBeforeNextDash;
    }

    void OnEnable()
    {
        _moveInput = playerActions.Player.Move;
        _moveInput.Enable();
        _sneakInput = playerActions.Player.Sneak;
        _sneakInput.Enable();
        _dashInput = playerActions.Player.Dash;
        _dashInput.Enable();
    }

    void OnDisable()
    {
        _moveInput.Disable();
        _sneakInput.Disable();
        _dashInput.Disable();
    }

    void ClampPositionToCamera()
    {
        float xOffset = 10;
        float yOffset = 5.5f;
        float clampedXPosition = Mathf.Clamp(_transform.position.x, -xOffset + _cam.position.x, xOffset + _cam.position.x);
        float clampedYPosition = Mathf.Clamp(_transform.position.y, -yOffset + _cam.position.y, yOffset + _cam.position.y);
        _transform.position = new Vector3(clampedXPosition, clampedYPosition, 0);
    }

    void Update()
    {
        _inputDirection = _moveInput.ReadValue<Vector2>();

        _currentDashSpeed = CalculateDashSpeed();
        _currentMoveSpeed = CalculateSmoothSpeed() + _currentDashSpeed * Time.deltaTime * 100;
        _currentDirection = CalculateSmoothDirection();

        _isDashing = _currentDashSpeed > 0 ? true : false;

        Move();
        RotateToDirection();
        ClampPositionToCamera();
        SetScale();
    }

    void SetScale()
    {
        _transform.localScale = _changeScale ? CalculateScaleFromSpeed() : _defaultScale;
    }

    Vector2 CalculateScaleFromSpeed()
    {
        Vector2 minScale =  _minimumSize * _defaultScale;
        float maxMoveSpeed = _moveSpeed + _dashSpeed;
        float speedScale = _currentMoveSpeed * _currentDirection.magnitude;

        return Vector2.Lerp(_defaultScale, minScale, speedScale / maxMoveSpeed);
    }


    void RotateToDirection()
    {
        Vector2 dir;

        if (_currentDirection == Vector2.zero)
        {
            _currentTimeToRotate = 0;
            dir = _lastDirection;
        }
        else
        {
            _currentTimeToRotate += Time.deltaTime;
            dir = Vector2.Lerp(_lastDirection, _currentDirection, _currentTimeToRotate / _timeToRotate);
        }

        float angle = _isRotatingToDirection ? Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg : 0;
        _transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    float CalculateSmoothSpeed()
    {
        float targetSpeed = _sneakInput.IsPressed() ? _moveSpeed * _sneakingAmount : _moveSpeed;

        if (_currentMoveSpeed != targetSpeed)
            _currentSpeedTransition = 0;

        _currentSpeedTransition += Time.deltaTime;

        return Mathf.Lerp(_currentMoveSpeed, targetSpeed, _currentSpeedTransition / _speedTransitionDuration);
    }

    float CalculateDashSpeed()
    {
        _currentTimeBeforeNextDash += Time.deltaTime;
        if (_dashInput.triggered && _currentTimeBeforeNextDash >= _timeBeforeNextDash && _currentDirection != Vector2.zero)
        {
            _currentTimeBeforeNextDash = 0;
            _currentDashDuration = 0;
            PlayDashParticles();
            PlayDashAudio();
        }

        // if is no more moving after dash, there will be no more dash effect.
        _currentDashDuration = _isMoving ? _currentDashDuration + Time.deltaTime : _dashDuration;

        return Mathf.Lerp(_dashSpeed, 0, _currentDashDuration / _dashDuration);
    }

    void PlayDashAudio() => SoundManager.Instance.PlayAllSound(_dashSounds);

    void PlayDashParticles()
    {
        _startDashParticle.transform.position = _transform.position;
        _startDashParticle.Play();
        _dashTrailParticle.Play();
    }

    Vector2 CalculateSmoothDirection()
    {
        if (_moveInput.triggered)
        {
            _currentCurveTime = 0;
            _isMoving = true;
        }

        if (_inputDirection == Vector2.zero && _isMoving)
        {
            _currentCurveTime = 0;
            _lastDirection = _currentDirection;
            _isMoving = false;
        }

        float curveValue = _isMoving ? CalculateCurveValue(_startingMoveCurve, _startingMoveDuration) : CalculateCurveValue(_endingMoveCurve, _currentEndingMoveDuration);
        return Vector2.Lerp(_currentDirection, _inputDirection.normalized, curveValue);
    }

    void Move() => _transform.Translate(Time.deltaTime * _currentMoveSpeed * _currentDirection, relativeTo: Space.World);

    float CalculateCurveValue(AnimationCurve curve, float duration)
    {
        if (_currentCurveTime >= duration)
            return curve.Evaluate(1);

        _currentCurveTime += Time.deltaTime;

        float xCurve = _currentCurveTime / duration;

        return curve.Evaluate(xCurve);
    }
}