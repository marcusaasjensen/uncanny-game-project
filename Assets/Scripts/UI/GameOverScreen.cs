using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class GameOverScreen : GameScreen
{
    [SerializeField] Image background;
    [SerializeField] float backgroundFadeSpeed;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] TextMeshProUGUI pressSpaceText;
    [SerializeField] float timeBetweenTexts = 1;
    
    Color _defaultSpaceTextColor;

    void Awake() => _defaultSpaceTextColor = pressSpaceText ? pressSpaceText.color : Color.white;

    IEnumerator Start()
    {
        if (background) background.color = new Color(background.color.r, background.color.b, background.color.g, 0);
        gameOverText.color = Color.clear;
        pressSpaceText.color = Color.clear;
        yield return new WaitForSeconds(timeBetweenTexts);
        yield return StartCoroutine(AppearText(gameOverText, Color.clear, Color.white));
        yield return new WaitForSeconds(timeBetweenTexts);
        yield return StartCoroutine(AppearText(pressSpaceText, Color.clear, _defaultSpaceTextColor));
    }

    void Update() => ShowGameOverBackground();

    void ShowGameOverBackground()
    {
        if (!background) return;
        if (background.color.a >= 225) return;
        background.color = new Color(background.color.r, background.color.b, background.color.g, background.color.a + backgroundFadeSpeed * Time.deltaTime);
    }
}
