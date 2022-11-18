using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Image frontHealthBar;
    public Image backHealthBar;
    public PlayerLife player;

    float _lerpTimer = 0f;
    float _chipSpeed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (backHealthBar != null && frontHealthBar != null)
            UpdateHealthUI();

        OnPlayerDamage();
        OnPLayerDeath();
    }

    void OnPLayerDeath() //Make event for player death
    {
        if (player.Health > 0) return;
        backHealthBar.fillAmount = 0;
        frontHealthBar.fillAmount = 0;
    }

    void OnPlayerDamage()
    {
        if(player.IsBeingDamaged)
            _lerpTimer = 0f;
    }


    public void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float healthFraction = player.Health / player.Maxhealth;
        if (fillB > healthFraction)
        {
            frontHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.red;
            _lerpTimer += Time.deltaTime;
            float percentComplete = _lerpTimer / _chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, healthFraction, percentComplete);
        }
    }
}
