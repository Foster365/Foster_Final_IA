using System.Collections.Generic;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "Leader Idle State", menuName = "Custom SO/FSM States/Leader States/Leader Idle State", order = 0)]
public class IdleState : State
{
    float timer = 2f;
    private Dictionary<EntityModel, CharacterModel> _entitiesData = new Dictionary<EntityModel, CharacterModel>();
    public override void EnterState(EntityModel model)
    {
        _entitiesData.Add(model, model as CharacterModel);
        //charModel.View.PlayWalkAnimation(false);
        _entitiesData[model].GetRigidbody().velocity = Vector3.zero;
    }

    public override void ExecuteState(EntityModel model)
    {
        timer -= Time.deltaTime;
        if (timer >= _entitiesData[model].CharAIData.IdleTimer) _entitiesData[model].IsIdle = false;
    }

    public override void ExitState(EntityModel model)
    {
        _entitiesData.Remove(model);
        timer = 0f;
    }
}