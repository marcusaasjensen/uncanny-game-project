using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandererLevelBoss : LevelBoss
{
    [SerializeField] WandererAttacks _boss;

    public override IEnumerator ActionSequence()
    {
        if (!_boss)
        {
            Debug.LogWarning("The BossController reference in LevelBoss script is missing.", this);
            yield break;
        }
        yield return StartCoroutine(_boss.ThrowStars(12, 1));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(_boss.ThrowBalloons(18, 2));
        print("finished");
        isLevelCompleted = true;
    }
}
