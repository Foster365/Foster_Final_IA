using System.Collections.Generic;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "NPC Attack State", menuName = "Custom SO/FSM States/NPC States/NPC Attack State", order = 0)]
public class NPCAttackState : State
{
    float timer = 0;
    private Dictionary<EntityModel, CharacterModel> _entitiesData = new Dictionary<EntityModel, CharacterModel>();
    public override void EnterState(EntityModel model)
    {
        Debug.Log("FSM NPC Attack START");
        _entitiesData.Add(model, model as CharacterModel);
        //charModel.View.PlayWalkAnimation(false);
        _entitiesData[model].GetRigidbody().velocity = Vector3.zero;
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log("FSM NPC Attack EXECUTE");

        timer += Time.deltaTime;

        if (timer <= _entitiesData[model].CharAIData.IdleTimer)
        {
            Debug.Log("Attack behaviour");
            //if(_entitiesData[model].GetComponent<CharacterController>().CharAIController.LineOfSight())_entitiesData[model].IsChasing = true;
        }
        else
        {
            timer = 0;
            _entitiesData[model].IsAttack = true;
        }
    }

    public override void ExitState(EntityModel model)
    {
        Debug.Log("FSM NPC Attack EXIT");
        _entitiesData.Remove(model);
    }
}