using System.Collections.Generic;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "NPC Seek State", menuName = "Custom SO/FSM States/NPC States/NPC Seek State", order = 0)]
public class NPCSeekState : State
{

    private Dictionary<EntityModel, DataMovementState> _entitiesData = new Dictionary<EntityModel, DataMovementState>();

    private class DataMovementState
    {
        public CharacterModel model;
        public CharacterController controller;

        public DataMovementState(EntityModel entityModel)
        {
            model = (CharacterModel)entityModel;
            controller = model.gameObject.GetComponent<CharacterController>();
        }
    }

    public override void EnterState(EntityModel model)
    {
        if (!_entitiesData.ContainsKey(model)) _entitiesData.Add(model, new DataMovementState(model));

        _entitiesData[model].model.gameObject.GetComponent<Leader>().target = _entitiesData[model].controller.CharAIController.Target;
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log(_entitiesData[model].model.gameObject.name + "FSM NPC Seek EXECUTE ");
        if(_entitiesData[model].controller.CharAIController.Target != null)
        {

            var dist = Vector3.Distance(_entitiesData[model].model.transform.position, _entitiesData[model].controller.CharAIController.Target.position);
            _entitiesData[model].model.View.CharacterMoveAnimation(true);

            _entitiesData[model].model.LookDir(_entitiesData[model].model.gameObject.GetComponent<FlockingManager>().RunFlockingDir() + _entitiesData[model].controller.CharAIController.SbPursuit.GetDir());
            _entitiesData[model].model.Move(_entitiesData[model].model.gameObject.GetComponent<FlockingManager>().RunFlockingDir() + _entitiesData[model].controller.CharAIController.SbPursuit.GetDir());

            HandleTransitionToAttackState(_entitiesData[model].model, dist);

        }
    }

    public override void ExitState(EntityModel model)
    {
        _entitiesData.Remove(model);
    }

    void HandleTransitionToAttackState(CharacterModel model, float dist)
    {
        if (dist < _entitiesData[model].model.Data.AttackRange)
        {
            _entitiesData[model].model.IsSeek = false;
            _entitiesData[model].model.IsAttack = true;
            //_entitiesData[model].model.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //_entitiesData[model].model.View.CharacterMoveAnimation(false);
        }
    }

}