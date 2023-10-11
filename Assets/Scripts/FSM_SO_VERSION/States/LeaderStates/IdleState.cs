using System.Collections.Generic;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "Leader Idle State", menuName = "Custom SO/FSM States/Leader States/Leader Idle State", order = 0)]
public class IdleState : State
{
    float timer = 0;
    private Dictionary<EntityModel, CharacterModel> _entitiesData = new Dictionary<EntityModel, CharacterModel>();
    public override void EnterState(EntityModel model)
    {
        _entitiesData.Add(model, model as CharacterModel);
        //charModel.View.PlayWalkAnimation(false);
        _entitiesData[model].GetRigidbody().velocity = Vector3.zero;
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log("Leader idle state execute");

        timer += Time.deltaTime;

        if(timer <= _entitiesData[model].CharAIData.IdleTimer)
        {
            Debug.Log("Idle behaviour");
            //if(_entitiesData[model].GetComponent<CharacterController>().CharAIController.LineOfSight())_entitiesData[model].IsChasing = true;
        }
        else
        {
            timer = 0;
            _entitiesData[model].IsPatrolling = true;
        }
    }

    public override void ExitState(EntityModel model)
    {
        _entitiesData.Remove(model);
    }
}