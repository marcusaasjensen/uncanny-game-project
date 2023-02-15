using System.Collections;
using UnityEngine;
using TMPro;
using System.Data;
using System;

public class WinScreen : GameScreen
{
    [SerializeField] TextMeshProUGUI levelCompletedText;
    [SerializeField] TextMeshProUGUI pressEnterText;

    IEnumerator Start()
    {
        levelCompletedText.color = Color.clear;
        pressEnterText.color = Color.clear;
        yield return StartCoroutine(AppearText(levelCompletedText, Color.clear, Color.white));
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(AppearText(pressEnterText, Color.clear, Color.white));
    }
}
