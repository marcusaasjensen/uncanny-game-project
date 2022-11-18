using UnityEngine;

[DisallowMultipleComponent]
public class SettingBehaviour : MonoBehaviour
{
    public AnimationCurveFunction animationCurveFunctions;
    public ProjectileMovement projectile;
    public RecordingPlayer rhythm;
    [Space]
    [Header("Curve")]
    [SerializeField] AnimationCurveTypes _curveType;
    [SerializeField] TimingBehaviour _timingBehaviour;
    [Space]
    [Header("Behaviour")]
    [SerializeField] [Min(0.1f)] float _period;
    [SerializeField] float _emphasize;
    [SerializeField] bool _periodDependsOnRhythm;
    [SerializeField] bool _smoothPeriodChanges;
    [Space]
    [Header("Settings to affect")]
    [SerializeField] bool _affectSpeed;
    [SerializeField] bool _affectDirection;
    [SerializeField] bool _affectSize;

    AnimationCurve _curve;
    float _elapsedTime;
    float _previousPeriod = 1;

    public AnimationCurveTypes CurveType
    {
        get { return _curveType; }
        set { _curveType = value; }
    }
    public float Period
    {
        get { return _period; }
        set { _period = value; }
    }
    public float Emphasize
    {
        get { return _emphasize; }
        set { _emphasize = value; }
    }
    public bool AffectSpeed
    {
        get { return _affectSpeed; }
        set { _affectSpeed = value; }
    }
    public bool AffectSize
    {
        get { return _affectSize; }
        set { _affectSize = value; }
    }
    public bool AffectDirection
    {
        get { return _affectDirection; }
        set { _affectDirection = value; }
    }
    public bool PeriodDependsOnRhythm
    {
        get { return _periodDependsOnRhythm; }
        set { _periodDependsOnRhythm = value; }
    }
    public bool SmoothPeriodChanges
    {
        get { return _smoothPeriodChanges; }
        set { _smoothPeriodChanges = value; }
    }
    public TimingBehaviour TimingBehaviour
    {
        get { return _timingBehaviour; }
        set { _timingBehaviour = value; }
    }

    void Awake() => animationCurveFunctions = AnimationCurveFunction.Instance;

    void Update()
    {
        if (animationCurveFunctions == null)
        {
            Debug.LogWarning("animation Curve Functions is not referenced or does not exist. Create a single GameObject with AnimationCurveFunction script associated. Reference the GameObject in this current script.");
            return;
        }
        _curve = animationCurveFunctions.GetCurve(_curveType);
        SetLerpFunction();
    }

    public void SetSpeedBehaviour(SettingBehaviour sb)
    {
        rhythm = sb.rhythm;
        _period = sb.Period;
        _curveType = sb.CurveType;
        _emphasize = sb.Emphasize;
        _timingBehaviour = sb.TimingBehaviour;
        _affectDirection = sb.AffectDirection;
        _affectSpeed = sb.AffectSpeed;
        _affectSize = sb.AffectSize;
        _periodDependsOnRhythm = sb.PeriodDependsOnRhythm;
        _smoothPeriodChanges = sb.SmoothPeriodChanges;
    }

    void SetLerpFunction()
    {
        if (_previousPeriod != _period)
            _previousPeriod = _period;

        _period = GetPeriodOnRhythm();

        if (_period < .1f)
            _period = .1f;

        _elapsedTime = ClampTimeToPeriod(CalculateTimingBehaviour(_timingBehaviour));

        float lerpRatio = _elapsedTime / _period;
        AffectChosenSetting(_curve.Evaluate(lerpRatio) * _emphasize);
    }

    void AffectChosenSetting(float curveValue)
    {
        if (_affectSpeed) { projectile.MoveSpeed *= curveValue; }
        if(_affectDirection) { projectile.Direction *= curveValue; }
        if (_affectSize) { projectile.Size *= curveValue; }
    }

    float GetPeriodOnRhythm()
    {
        if (rhythm == null) { 
            Debug.LogWarning("Rhythm is not referenced. Check reference in Spawned projectile prefab and in all TMP projectile."); 
            return _period; 
        }

        MouseClickingRecorder.Recording recording = rhythm.recordingDictionary[rhythm.recordingTagToPlay];

        if (_periodDependsOnRhythm && recording.index < recording.timeBetweenClicks.Count - 1)
            return (float)recording.timeBetweenClicks[recording.index];
        else
            return _period;
    }

    float CalculateTimingBehaviour(TimingBehaviour behaviourName)
    {
        return behaviourName switch
        {
            TimingBehaviour.ShiftedTime => Time.time + projectile.AppearanceTime,
            TimingBehaviour.RealTime => Time.time,
            TimingBehaviour.EachTime => projectile.AppearanceTime,
            _ => default,
        };
    }

    float ClampTimeToPeriod(float time)
    {
        float currentPeriod = _smoothPeriodChanges ? CalculateSmoothPeriodChange() : _period;
        float clampedTime = time / currentPeriod % 1;
        return clampedTime;
    }

    float CalculateSmoothPeriodChange()
    {
        float velocity = 0.0f;
        float smoothedPeriod = Mathf.SmoothDamp(_previousPeriod, _period, ref velocity, _previousPeriod);
        return smoothedPeriod;
    }
}
