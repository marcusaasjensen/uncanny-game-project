using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] float _fadeSpeed;

    void Start()
    {
        img = GetComponent<Image>();
        if (img) img.color = new Color(img.color.r, img.color.b, img.color.g, 0);
    }

    void Update()
    {
        ShowOnScreenByFade();
    }

    void ShowOnScreenByFade() {
        if (img.color.a >= 225) return;
        img.color = new Color(img.color.r, img.color.b, img.color.g, img.color.a + _fadeSpeed * Time.deltaTime);
    }
}
