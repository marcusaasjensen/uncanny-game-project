using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    [System.Serializable]
    public class ProjectileAttack
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public ProjectileController AttackToDeploy { get; private set; }
        [field: SerializeField] public GameObject AttackHint { get; private set; }

        public IEnumerator ShowHint(float hintTime)
        {
            if (!AttackHint) yield break;

            AttackHint.SetActive(true);
            yield return new WaitForSeconds(hintTime);
            AttackHint.SetActive(false);
        }

        public IEnumerator StartAttack(float timeToAttack)
        {
            if(!AttackToDeploy) yield break;

            AttackToDeploy.gameObject.SetActive(true);
            yield return new WaitForSeconds(timeToAttack);
            AttackToDeploy.gameObject.SetActive(false);
        }
    }

    [SerializeField] BossController _bossController;
    [SerializeField] List<ProjectileAttack> _attacks;

    Dictionary<string, ProjectileAttack> _attackMap;

    void Awake() 
    { 
        if(!_bossController)
            _bossController = GetComponent<BossController>();

        InstiantiateAttacks();
        SetAttacksToInactive();
    }

    void SetAttacksToInactive()
    {
        foreach (var attack in _attacks) 
        { 
            if(attack.AttackToDeploy) attack.AttackToDeploy.gameObject.SetActive(false);
            if(attack.AttackHint) attack.AttackHint.SetActive(false);
        }
    }

    IEnumerator DeployAttack(ProjectileAttack attack, float time, float hintTimeBeforeAttack)
    {
        yield return StartCoroutine(attack.ShowHint(hintTimeBeforeAttack));
        yield return StartCoroutine(attack.StartAttack(time));
    }

    void InstiantiateAttacks()
    {
        _attackMap = new Dictionary<string, ProjectileAttack>();
        foreach(ProjectileAttack attack in _attacks) _attackMap.Add(attack.Name, attack);
    }

    public IEnumerator ThrowStars(float attackTime, float hintTimeBeforeAttack)
    {
        //animation for boss dancing, boss hint before attack, etc...
        ProjectileAttack starsAttack = _attackMap["Stars"];
        yield return StartCoroutine(DeployAttack(starsAttack, attackTime, hintTimeBeforeAttack));
    }

    public IEnumerator ThrowBalloons(float attackTime, float hintTimeBeforeAttack)
    {
        //""
        ProjectileAttack balloonsAttack = _attackMap["Balloons"];
        yield return StartCoroutine(DeployAttack(balloonsAttack, attackTime, hintTimeBeforeAttack));
    }

}
