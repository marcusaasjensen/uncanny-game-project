using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelBoss : MonoBehaviour
{
    [SerializeField] List<GameObject> _levelElements;

    [ContextMenu("Start Sequence")]
    public void StartActionSequence() => StartCoroutine(ActionSequence());
    public void StopActionSequence() => StopCoroutine(ActionSequence());
    public abstract IEnumerator ActionSequence();
}
