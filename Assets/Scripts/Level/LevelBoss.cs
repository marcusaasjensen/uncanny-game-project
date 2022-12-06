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
    void StartActionSequence() => StartCoroutine(ActionSequence());

    public IEnumerator ActionSequence()
    {
        if (!_boss)
        {
            Debug.LogWarning("The BossController reference in LevelBoss script is missing.", this);
            yield break;
        }

        yield return StartCoroutine(_boss.AttackController.ThrowBalloons(2));
        yield return new WaitForSeconds(5f);
        yield return StartCoroutine(_boss.AttackController.ThrowStars(5));
        print("finished");
    }
}
