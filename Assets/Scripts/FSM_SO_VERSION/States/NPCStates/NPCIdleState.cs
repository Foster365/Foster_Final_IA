using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "NPC Idle State", menuName = "Custom SO/FSM States/NPC States/NPC Idle State", order = 0)]
public class NPCIdleState : State
{
    float timer = 0;
    private Dictionary<EntityModel, CharacterModel> _entitiesData = new Dictionary<EntityModel, CharacterModel>();
    public GameObject boss;
    public override void EnterState(EntityModel model)
    {
        Debug.Log("FSM NPC IDLE START");
        _entitiesData.Add(model, model as CharacterModel);
        //charModel.View.PlayWalkAnimation(false);
        _entitiesData[model].GetRigidbody().velocity = Vector3.zero;

        if(boss == null)
        {
            boss = GameObject.Find(_entitiesData[model].Data.BossName);
        }
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log("FSM NPC IDLE EXECUTE");

        timer += Time.deltaTime;

        if (boss.gameObject.GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAA AMO A MI ESPOSA");
            _entitiesData[model].IsFollowLeader = true;
        }
    }

    public override void ExitState(EntityModel model)
    {
        Debug.Log("FSM NPC IDLE EXIT");
        _entitiesData.Remove(model);
    }
}