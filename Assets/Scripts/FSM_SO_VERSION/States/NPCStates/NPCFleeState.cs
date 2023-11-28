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
        public List<Node> nodesToTarget;
        public Grid grid;

        public DataFleeState(EntityModel entityModel)
        {
            model = (CharacterModel)entityModel;
            controller = model.gameObject.GetComponent<CharacterController>();
            grid = model.MapGrid;
            nodesToTarget = new List<Node>();
        }
    }

    public override void EnterState(EntityModel model)
    {
        _entitiesData.Add(model, new DataFleeState(model));
        _entitiesData[model].model.View.CharacterMoveAnimation(true);
        //charModel.View.PlayWalkAnimation(false);
        //_entitiesData[model].model.GetRigidbody().velocity = Vector3.zero;
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log(_entitiesData[model].model.gameObject.name + "FSM NPC Flee EXECUTE");

        _entitiesData[model].model.LookDir(_entitiesData[model].controller.CharAIController.SbFlee.GetDir() + _entitiesData[model].model.gameObject.GetComponent<FlockingManager>().RunFlockingDir());
        _entitiesData[model].model.Move(_entitiesData[model].controller.CharAIController.SbFlee.GetDir() + _entitiesData[model].model.gameObject.GetComponent<FlockingManager>().RunFlockingDir());

        _entitiesData[model].model.HealthController.Heal(_entitiesData[model].model.Data.HealthRegenerationAmount); //Regenerate while fleeing from target

        if(timer >= _entitiesData[model].model.CharAIData.FleeStateTimer)
        {
            _entitiesData[model].model.IsFlee = false;
            _entitiesData[model].model.IsFollowLeader = true;
        }

    }

    public override void ExitState(EntityModel model)
    {
        _entitiesData.Remove(model);
    }
}