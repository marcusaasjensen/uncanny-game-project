using System.Collections.Generic;
using UnityEngine;

public class AnimationCurveFunction : MonoBehaviour
{
    public static AnimationCurveFunction Instance;
    public List<AnimationCurve> animationCurves;

    public AnimationCurve GetCurve(AnimationCurveType type)
    {
        return animationCurves[(int)type];
    }

    void Awake() { if(Instance == null) Instance = this; }
}