using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class LevelTutorial : MonoBehaviour
{
    [SerializeField] PlayerLife player;
    [SerializeField] float timebetweenRespawns = .5f;
    [SerializeField] Vector3 startPosition;
    [SerializeField] private Image controlsImage;
    [SerializeField] private TextMeshProUGUI playerText;
    [SerializeField] private List<GameObject> exampleAttacks;
    [SerializeField] private Vector3 textOffset;
    [SerializeField] private AudioClip nextTextSound;
    

    void Start()
    {
        LevelEvents.level.OnGameOver += RespawnPlayer;
        StartCoroutine(TutorialCoroutine());
    }

    void Update()
    {
        playerText.transform.position = player.transform.position + textOffset;
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

    IEnumerator TutorialCoroutine()
    {
        ShowText("Welcome");
        var movement = player.GetComponent<PlayerMovement>();
        movement.enabled = false;
        yield return new WaitForSeconds(3);
        
        ShowText("You are that cube.");
        yield return new WaitForSeconds(2f);
        
        ShowText("Try moving with the controls (ZQSD).");
        movement.enabled = true;
        controlsImage.enabled = true;
        
        yield return new WaitUntil(() => movement.IsMoving);
        yield return new WaitForSeconds(2f);
        ShowText("Great! Now, try to dash WHILE moving (Space).");

        yield return new WaitUntil(() => movement.IsDashing);
        yield return new WaitForSeconds(4f);
        RespawnPlayer();
        exampleAttacks[0].SetActive(true);
        ShowText("Perfect!");
        yield return new WaitForSeconds(2f);
        ShowText("Now... You see that blue cube cascade?");
        yield return new WaitForSeconds(5f);
        ShowText("Try dashing through it to get to the other side.");
        
        yield return new WaitUntil(() => player.transform.position.x > 5);
        ShowText("Well done.");
        yield return new WaitForSeconds(2f);
        ShowText("Yup, you don't take damages while dashing.");
        startPosition = new Vector3(6,0,0);
        yield return new WaitForSeconds(5f);
        RespawnPlayer();
        ShowText("Now, go back to where you were.");
        exampleAttacks[1].SetActive(true);
        yield return new WaitUntil(() => player.transform.position.x < -3);
        yield return new WaitForSeconds(1f);
        exampleAttacks[0].SetActive(false);
        exampleAttacks[1].SetActive(false);
        ShowText("Congratulations.");
        startPosition = new Vector3(0,0,0);
        yield return new WaitForSeconds(2f);
        ShowText("That's all you need to know.");
        yield return new WaitForSeconds(2f);
        ShowText("Just one advice...");
        yield return new WaitForSeconds(1.5f);
        ShowText("Keep moving and dashing through.");
        yield return new WaitForSeconds(3f);
        ShowText("Watch out!");
        exampleAttacks[2].SetActive(true);
        yield return new WaitForSeconds(2f);
        ShowText("");
        yield return new WaitForSeconds(3f);
        exampleAttacks[2].SetActive(false);
        LevelEvents.level.LevelFinished();
    }

    void ShowText(string text)
    {
        SoundManager.Instance.PlaySound(nextTextSound);
        playerText.text = text;
    }
}
