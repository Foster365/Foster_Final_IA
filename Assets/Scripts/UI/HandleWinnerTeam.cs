using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandleWinnerTeam : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI winnerTeamTxt;
    [SerializeField] TextMeshProUGUI teamWinsTxt;
    [SerializeField] CharacterModel leaderA;
    [SerializeField] CharacterModel leaderB;

    // Update is called once per frame
    void Update()
    {
        CheckWinnerTeam();
    }

    void CheckWinnerTeam()
    {
        if(leaderA.HealthController.IsDead)
        {
            teamWinsTxt.gameObject.SetActive(true);
            winnerTeamTxt.text = "Dogs";
        }
        else if (leaderB.HealthController.IsDead)
        {
            teamWinsTxt.gameObject.SetActive(true);
            winnerTeamTxt.text = "Devil's";
        }
    }
}
