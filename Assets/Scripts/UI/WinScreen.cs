using System.Collections;
using UnityEngine;
using TMPro;
using System.Data;
using System;
using UnityEngine.UI;

public class WinScreen : GameScreen
{
    [SerializeField] TextMeshProUGUI levelCompletedText;
    [SerializeField] TextMeshProUGUI pressEnterText; 
    [SerializeField] TextMeshProUGUI pressRText;
    Color _defaultEnterTextColor;

    void Awake() => _defaultEnterTextColor = pressEnterText ? pressEnterText.color : Color.white;    

    IEnumerator Start()
    {
        levelCompletedText.color = Color.clear;
        pressEnterText.color = Color.clear;
        pressRText.color = Color.clear;

        yield return StartCoroutine(AppearText(levelCompletedText, Color.clear, Color.white));
        yield return new WaitForSeconds(1);
        StartCoroutine(AppearText(pressEnterText, Color.clear, _defaultEnterTextColor));
        StartCoroutine(AppearText(pressRText, Color.clear, _defaultEnterTextColor));
    }
}
