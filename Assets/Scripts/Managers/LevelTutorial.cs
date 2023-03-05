using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTutorial : MonoBehaviour
{
    [SerializeField] PlayerLife player;
    [SerializeField] float timebetweenRespawns = .5f;
    [SerializeField] Vector3 startPosition;

    void Start()
    {
        LevelEvents.level.OnGameOver += RespawnPlayer;    
    }

    void RespawnPlayer()
    {
        StartCoroutine(RespawnPlayerCoroutine());
    }

    IEnumerator RespawnPlayerCoroutine()
    {
        player.transform.SetPositionAndRotation(startPosition, Quaternion.identity);
        yield return new WaitForSeconds(timebetweenRespawns);
        player.gameObject.SetActive(true);
        player.FillHealth();
    }
}
