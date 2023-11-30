using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "Leader Idle State", menuName = "Custom SO/FSM States/Leader States/Leader Idle State", order = 0)]
public class IdleState : State
{
    float idleStateTimer = 0;
    private Dictionary<EntityModel, IdleData> _entitiesData = new Dictionary<EntityModel, IdleData>();

    public class IdleData
    {
        public CharacterModel model;
        public IdleData(EntityModel _model)
        {
            model = _model as CharacterModel;
        }
    }

    public override void EnterState(EntityModel model)
    {
        _entitiesData.Add(model, new IdleData(model));

        _entitiesData[model].model.GetRigidbody().velocity = Vector3.zero;
        _entitiesData[model].model.View.CharacterMoveAnimation(false);
        _entitiesData[model].model.HealthController.CanReceiveDamage = true;
    }
    
    public override void ExecuteState(EntityModel model)
    {
        //Debug.Log("FSM Leader IDLE EXECUTE " + _entitiesData[model].model.gameObject.name);

        idleStateTimer += Time.deltaTime;

        CheckTransitionToDeathState(_entitiesData[model].model);
        
        if (idleStateTimer > _entitiesData[model].model.CharAIData.IdleTimer)
        {
            _entitiesData[model].model.IsPatrolling = true;
            idleStateTimer = 0;
        }
    }

    public override void ExitState(EntityModel model)
    {
        _entitiesData.Remove(model);
    }

    void CheckTransitionToDeathState(EntityModel model)
    {
        if (_entitiesData[model].model.HealthController.CurrentHealth <= 0)
            _entitiesData[model].model.HealthController.IsDead = true;
    }

}