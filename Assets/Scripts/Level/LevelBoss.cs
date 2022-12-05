using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBoss : MonoBehaviour
{
    [SerializeField] Clown _boss;
    [SerializeField] List<GameObject> _levelElements;
    
    protected IEnumerator StartActionSequence()
    {
        yield return null;
    }
}
