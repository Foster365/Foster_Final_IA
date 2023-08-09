using System.Collections.Generic;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "Leader Idle State", menuName = "Custom SO/FSM States/Leader States/Leader Idle State", order = 0)]
public class IdleState : State
{
    float timer;
    private Dictionary<EntityModel, CharacterModel> _entitiesData = new Dictionary<EntityModel, CharacterModel>();
    public override void EnterState(EntityModel model)
    {
        _entitiesData.Add(model, model as CharacterModel);
        timer = 0;
        //charModel.View.PlayWalkAnimation(false);
        _entitiesData[model].GetRigidbody().velocity = Vector3.zero;
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log("Leader idle state execute");
        Debug.Log("Timer: " + timer);
        if (timer <= _entitiesData[model].CharAIData.IdleTimer) timer += Time.deltaTime;
        else
        {
            Debug.Log("Not idle aaaaaaaaaaa");
            _entitiesData[model].IsPatrolling = true;
        }
    }

    public override void ExitState(EntityModel model)
    {
        _entitiesData.Remove(model);
        timer = 0;
    }
}