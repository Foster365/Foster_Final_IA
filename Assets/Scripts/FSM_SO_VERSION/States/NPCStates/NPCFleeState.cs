using System.Collections.Generic;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "NPC Flee State", menuName = "Custom SO/FSM States/NPC States/NPC Flee State", order = 0)]
public class NPCFleeState : State
{
    float timer = 0;
    private Dictionary<EntityModel, CharacterModel> _entitiesData = new Dictionary<EntityModel, CharacterModel>();
    public override void EnterState(EntityModel model)
    {
        Debug.Log("FSM NPC Flee START");
        _entitiesData.Add(model, model as CharacterModel);
        //charModel.View.PlayWalkAnimation(false);
        _entitiesData[model].GetRigidbody().velocity = Vector3.zero;
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log("FSM NPC Flee EXECUTE");

        timer += Time.deltaTime;

        if (timer <= _entitiesData[model].CharAIData.IdleTimer)
        {
            Debug.Log("Flee behaviour");
            //if(_entitiesData[model].GetComponent<CharacterController>().CharAIController.LineOfSight())_entitiesData[model].IsChasing = true;
        }
        else
        {
            timer = 0;
            _entitiesData[model].IsFlee = true;
        }
    }

    public override void ExitState(EntityModel model)
    {
        Debug.Log("FSM NPC Flee EXIT");
        _entitiesData.Remove(model);
    }
}