using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthRingColourUIScript : MonoBehaviour
{
    public Image healthBar;

    float health, maxHealth;
    float lerpSpeed;

    [SerializeField]CharacterModel charModel;

    private void Start()
    {
        health = charModel.HealthController.CurrentHealth;
        maxHealth = charModel.HealthController.MaxHealth;
    }

    private void Update()
    {
        HealthBarFiller();

        lerpSpeed = 3f * Time.deltaTime;
    }

    void HealthBarFiller()
    {
        Debug.Log(health);
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount,health / maxHealth, lerpSpeed);
    }

    void ColorChange()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (health / maxHealth));

        healthBar.color = healthColor;
    }
}
