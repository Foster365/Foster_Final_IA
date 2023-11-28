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
    [SerializeField] TextMeshProUGUI blockingAttacksUITxt;

    private void Start()
    {
        if(character.HealthController != null) txtUI.text = character.HealthController.MaxHealth.ToString();
    }

    private void Update()
    {
        if (character.HealthController.CurrentHealth <= 0f) //TODO fix vida menor a 0 renderizandose en la Health UI
            txtUI.text = "0";

        txtUI.text = character.HealthController.CurrentHealth.ToString();

        HandleBlockingUIText();

    }

    void HandleBlockingUIText()
    {
        if (!character.HealthController.CanReceiveDamage) blockingAttacksUITxt.gameObject.SetActive(true);
        else blockingAttacksUITxt.gameObject.SetActive(false);
    }

}
