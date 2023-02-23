using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] List<GameObject> visualEffects;

    public void OnVFXActive(bool active)
    {
        if (visualEffects == null || visualEffects.Count == 0) return;
        foreach(GameObject vfx in visualEffects) vfx.SetActive(active);
    }
}
