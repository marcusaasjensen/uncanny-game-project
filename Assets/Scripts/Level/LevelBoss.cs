using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBoss : MonoBehaviour
{
    [SerializeField] BossController _boss;
    [SerializeField] List<GameObject> _levelElements;


    void Awake()
    {
        if(!_boss) _boss = FindObjectOfType<BossController>();
    }

    [ContextMenu("Start Sequence")]
    public void StartActionSequence() => StartCoroutine(ActionSequence());
    public void StopActionSequence() => StopCoroutine(ActionSequence());

    IEnumerator ActionSequence()
    {
        if (!_boss)
        {
            Debug.LogWarning("The BossController reference in LevelBoss script is missing.", this);
            yield break;
        }
        yield return StartCoroutine(_boss.AttackController.ThrowStars(12, 1));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(_boss.AttackController.ThrowBalloons(18, 2));
        print("finished");
    }
}
