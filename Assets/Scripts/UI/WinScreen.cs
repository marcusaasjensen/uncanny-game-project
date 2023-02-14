using System.Collections;
using UnityEngine;
using TMPro;
using System.Data;
using System;

public class WinScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelCompletedText;
    [SerializeField] TextMeshProUGUI pressEnterText;
    [SerializeField] float duration = 2f;

    IEnumerator Start()
    {
        levelCompletedText.color = Color.clear;
        pressEnterText.color = Color.clear;
        yield return StartCoroutine(AppearText(levelCompletedText, Color.clear, Color.white));
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(AppearText(pressEnterText, Color.clear, Color.white));
    }

    IEnumerator AppearText(TextMeshProUGUI textDisplay, Color fromColor, Color toColor)
    {
        if (!textDisplay) yield break;
        float currentTime = 0f;
        while (currentTime < duration)
        {
            print(textDisplay.color);
            textDisplay.color = Color.Lerp(fromColor, toColor, currentTime / duration);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}
