using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIHandler : MonoBehaviour
{
    [SerializeField] CharacterModel character;
    [SerializeField] Image textUI;
    [SerializeField] TextMeshProUGUI txtUI;

    private void Start()
    {
        if(character.HealthController != null) txtUI.text = character.HealthController.MaxHealth.ToString();
    }

    private void Update()
    {
        if (character.HealthController.CurrentHealth <= 0f) //TODO fix vida menor a 0 renderizandose en la Health UI
            txtUI.text = "0";

        txtUI.text = character.HealthController.CurrentHealth.ToString();
        //if (character.CharacterHealthController.CurrentHealth < 0f)
        //    textUI.fillAmount = 0;
        //Debug.Log("MAX: " + character.CharacterHealthController.MaxHealth + "CURR: " + character.CharacterHealthController.CurrentHealth);
        //textUI.fillAmount = character.CharacterHealthController.CurrentHealth / character.CharacterHealthController.MaxHealth;
        //Debug.Log("VIDA: " + textUI.fillAmount);
    }
}
