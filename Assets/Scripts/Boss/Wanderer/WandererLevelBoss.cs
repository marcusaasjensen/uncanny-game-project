using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandererLevelBoss : LevelBoss
{
    [SerializeField] WandererAttacks _boss;
    [SerializeField] Animator _bossAnimator;

    void Awake()
    {
        if(!_bossAnimator) _bossAnimator = _boss.GetComponent<Animator>();
    }

    public override IEnumerator ActionSequence()
    {
        if (!_boss)
        {
            Debug.LogWarning("The BossController reference in LevelBoss script is missing.", this);
            yield break;
        }
        yield return StartCoroutine(_boss.ThrowStars(12, 1));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(_boss.ThrowBalloons(17, 2));
        _bossAnimator.Play("Walking");
        yield return new WaitForSeconds(7f);
        _bossAnimator.Play("Idle");
        print("finished");
        isLevelCompleted = true;
    }
}
