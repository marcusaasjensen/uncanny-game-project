using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class GameScreen : MonoBehaviour
{
    [SerializeField] float duration = 2f;

    protected IEnumerator AppearText(TextMeshProUGUI textDisplay, Color fromColor, Color toColor)
    {
        if (!textDisplay) yield break;
        float currentTime = 0f;
        while (currentTime < duration)
        {
            textDisplay.color = Color.Lerp(fromColor, toColor, currentTime / duration);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}
