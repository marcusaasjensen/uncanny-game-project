using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBoss : MonoBehaviour
{
    [SerializeField] BossController _boss;
    [SerializeField] List<GameObject> _levelElements;
    
    public IEnumerator StartActionSequence()
    {
        yield return new WaitForSeconds(1f);
    }
}
