using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandererLevelBoss : LevelBoss
{
    [SerializeField] WandererAttacks _boss;

    public override IEnumerator ActionSequence()
    {
        _boss.GetComponent<Animator>().enabled = false;
        if (!_boss)
        {
            Debug.LogWarning("The BossController reference in LevelBoss script is missing.", this);
            yield break;
        }
        yield return StartCoroutine(_boss.ThrowStars(12, 1));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(_boss.ThrowBalloons(17, 2));
        _boss.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(10f);
        print("finished");
        isLevelCompleted = true;
    }
}
