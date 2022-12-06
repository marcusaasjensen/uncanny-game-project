using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [System.Serializable]
    public class ProjectileAttack
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public ProjectileController AttackToDeploy { get; private set; }
    }

    [SerializeField] List<ProjectileAttack> _attacks;

    Dictionary<string, ProjectileAttack> _attackMap;

    void Awake() => InstiantiateAttacks();

    IEnumerator DeployAttack(GameObject go, float time)
    {
        go.SetActive(true);
        yield return new WaitForSeconds(time);
        go.SetActive(false);
    }

    void InstiantiateAttacks()
    {
        _attackMap = new Dictionary<string, ProjectileAttack>();
        foreach(ProjectileAttack attack in _attacks) _attackMap.Add(attack.Name, attack);
    }

    public IEnumerator ThrowStars(float time)
    {
        yield return StartCoroutine(DeployAttack(_attackMap["Stars"].AttackToDeploy.gameObject, time));
    }

    public IEnumerator ThrowBalloons(float time)
    {
        yield return StartCoroutine(DeployAttack(_attackMap["Balloons"].AttackToDeploy.gameObject, time));
    }

}
