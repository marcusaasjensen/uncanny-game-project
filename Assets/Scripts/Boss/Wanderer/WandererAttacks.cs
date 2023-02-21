using UnityEngine;
using System;
using System.Collections;

public class WandererAttacks : BossAttacks
{
    public IEnumerator ThrowStars(float attackTime, float hintTimeBeforeAttack)
    {
        //animation for boss dancing, boss hint before attack, etc...
        ProjectileAttack starsAttack = _attackMap["Stars"];
        yield return StartCoroutine(DeployAttack(starsAttack, attackTime, hintTimeBeforeAttack));
    }

    public IEnumerator ThrowBalloons(float attackTime, float hintTimeBeforeAttack)
    {
        ProjectileAttack balloonsAttack = _attackMap["Balloons"];
        yield return StartCoroutine(DeployAttack(balloonsAttack, attackTime, hintTimeBeforeAttack));
    }
}
