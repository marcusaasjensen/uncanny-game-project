using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UncannyLevelBoss : LevelBoss
{
    [SerializeField] UncannyAttacks _boss;

    public override IEnumerator ActionSequence()
    {
        if (!_boss)
        {
            Debug.LogWarning("The BossController reference in LevelBoss script is missing.", this);
            yield break;
        }
        yield return new WaitForSeconds(1f);
        print("finished");
    }
}
