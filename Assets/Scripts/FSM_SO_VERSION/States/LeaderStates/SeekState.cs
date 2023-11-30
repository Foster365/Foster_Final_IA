using _Main.Scripts.FSM_SO_VERSION;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Leader Seek State", menuName = "Custom SO/FSM States/Leader States/Leader Seek State", order = 0)]
public class SeekState : State
{

    private Dictionary<EntityModel, DataMovementState> _entitiesData = new Dictionary<EntityModel, DataMovementState>();

    private class DataMovementState
    {
        public CharacterModel model;
        public CharacterController controller;
        public List<Node> nodesToTarget;
        public Grid grid;

        public DataMovementState(EntityModel entityModel)
        {
            model = (CharacterModel)entityModel;
            controller = model.gameObject.GetComponent<CharacterController>();
            grid = model.MapGrid;
            nodesToTarget = new List<Node>();
        }
    }

    public override void EnterState(EntityModel model)
    {
        if (!_entitiesData.ContainsKey(model)) _entitiesData.Add(model, new DataMovementState(model));
        if(_entitiesData[model].controller.CharAIController.Target != null)
        _entitiesData[model].model.IsBattleBegun = true;
        _entitiesData[model].model.View.CharacterMoveAnimation(true);
        _entitiesData[model].model.HealthController.CanReceiveDamage = true;
    }

    public override void ExecuteState(EntityModel model)
    {
        //Debug.Log("FSM Leader Seek EXECUTE " + _entitiesData[model].model.gameObject.name);

        model.LookDir(_entitiesData[model].controller.CharAIController.SbObstacleAvoidance.GetDir() + _entitiesData[model].controller.CharAIController.SbPursuit.GetDir());
        model.Move(_entitiesData[model].controller.CharAIController.SbObstacleAvoidance.GetDir() + _entitiesData[model].controller.CharAIController.SbPursuit.GetDir());

        var dist = Vector3.Distance(_entitiesData[model].model.transform.position, _entitiesData[model].controller.CharAIController.Target.position);

        HandleTransitionToAttackState(dist, _entitiesData[model].model);

    }

    public override void ExitState(EntityModel model)
    {
        _entitiesData.Remove(model);
    }

    public void HandleTransitionToAttackState(float dist, CharacterModel model)
    {
        if (dist < _entitiesData[model].model.Data.AttackRange)
        {
            _entitiesData[model].model.IsAttacking = true;
            _entitiesData[model].model.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            _entitiesData[model].model.View.CharacterMoveAnimation(false);
        }
    }
}