using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAttacks : MonoBehaviour
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

    [SerializeField] float _damageToPlayer = 30f;
    [SerializeField] List<ProjectileAttack> _attacks;
    protected Dictionary<string, ProjectileAttack> _attackMap;

    public float DamageToPlayer { get { return _damageToPlayer; } }


    void Awake() 
    { 
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

    protected IEnumerator DeployAttack(ProjectileAttack attack, float time, float hintTimeBeforeAttack)
    {
        yield return StartCoroutine(attack.ShowHint(hintTimeBeforeAttack));
        yield return StartCoroutine(attack.StartAttack(time));
    }

    void InstiantiateAttacks()
    {
        _attackMap = new Dictionary<string, ProjectileAttack>();
        foreach(ProjectileAttack attack in _attacks) _attackMap.Add(attack.Name, attack);
    }
}
