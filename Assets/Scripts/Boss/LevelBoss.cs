using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelBoss : MonoBehaviour
{
    [SerializeField] List<GameObject> _levelElements;
    public static bool isLevelCompleted = false;

    IEnumerator _actionSequenceCoroutine;


    void Start()
    {
        this.gameObject.SetActive(true);    
    }

    [ContextMenu("Start Sequence")]
    public void StartActionSequence()
    {
         StartCoroutine(ActionSequence());
        _actionSequenceCoroutine = ActionSequence();
    }
    public void StopActionSequence() 
    { 
        StopCoroutine(_actionSequenceCoroutine);
        this.gameObject.SetActive(false);
    }
    public abstract IEnumerator ActionSequence();
}
