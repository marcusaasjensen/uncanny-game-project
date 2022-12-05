using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBoss : MonoBehaviour
{
    [SerializeField] Clown _boss;
    [SerializeField] 
    
    protected IEnumerator StartActionSequence()
    {
        yield return null;
    }
}
