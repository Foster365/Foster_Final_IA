using System.Collections.Generic;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "NPC Flee State", menuName = "Custom SO/FSM States/NPC States/NPC Flee State", order = 0)]
public class NPCFleeState : State
{
    float timer = 0;
    private Dictionary<EntityModel, DataFleeState> _entitiesData = new Dictionary<EntityModel, DataFleeState>();

    private class DataFleeState
    {
        public CharacterModel model;
        public CharacterController controller;
        public float initSpeed;

        public DataFleeState(EntityModel entityModel)
        {
            model = (CharacterModel)entityModel;
            controller = model.gameObject.GetComponent<CharacterController>();
            initSpeed = model.CharAIData.FleeReferenceMovementSpeed;
            //model.Data.MovementSpeed -= (model.Data.MovementSpeed * .75f);
        }
    }

    public override void EnterState(EntityModel model)
    {
        _entitiesData.Add(model, new DataFleeState(model));
        _entitiesData[model].model.View.CharacterMoveAnimation(true);
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log(_entitiesData[model].model.gameObject.name + "FSM NPC Flee EXECUTE");
        timer += Time.deltaTime;

        if(_entitiesData[model].controller.CharAIController.Target != null)
        {
            _entitiesData[model].model.LookDir(_entitiesData[model].controller.CharAIController.SbFlee.GetDir() + _entitiesData[model].model.gameObject.GetComponent<FlockingManager>().RunFlockingDir());
            _entitiesData[model].model.Move(_entitiesData[model].controller.CharAIController.SbFlee.GetDir() + _entitiesData[model].model.gameObject.GetComponent<FlockingManager>().RunFlockingDir());
            Debug.Log("flee curr speed" + _entitiesData[model].model.Data.MovementSpeed);
            //_entitiesData[model].model.HealthController.Heal(_entitiesData[model].model.Data.HealthRegenerationAmount); //Regenerate while fleeing from target
            Debug.Log(_entitiesData[model].model.HealthController.CurrentHealth + "health de npc paso a flee");

        }

        HandleTransitionToFollowLeaderState(_entitiesData[model].model);
        CheckTransitionToDeathState(_entitiesData[model].model);
    }

    public override void ExitState(EntityModel model)
    {
        _entitiesData.Remove(model);
    }

    void HandleTransitionToFollowLeaderState(CharacterModel model)
    {
        if (timer > _entitiesData[model].model.CharAIData.FleeStateTimer)
        {
            _entitiesData[model].model.IsFlee = false;
            _entitiesData[model].model.IsSearching = true;
            _entitiesData[model].model.Data.MovementSpeed = _entitiesData[model].model.CharAIData.FleeReferenceMovementSpeed;
            Debug.Log("flee speed reset" + _entitiesData[model].model.Data.MovementSpeed);

            timer = 0;
        }

    }

    public void CheckTransitionToDeathState(CharacterModel model)
    {
        if (_entitiesData[model].model.HealthController.CurrentHealth <= 0)
        {
            _entitiesData[model].model.IsDead = true;
            _entitiesData[model].model.IsFlee = false;
            timer = 0;
        }
    }

}