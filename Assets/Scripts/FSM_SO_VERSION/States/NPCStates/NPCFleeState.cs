using System.Collections.Generic;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "NPC Flee State", menuName = "Custom SO/FSM States/NPC States/NPC Flee State", order = 0)]
public class NPCFleeState : State
{
    float timer = 0;
    private Dictionary<EntityModel,DataFleeState> _entitiesData = new Dictionary<EntityModel, DataFleeState>();
    ISteeringBehaviour _steering;
    Flee flee;

    private class DataFleeState
    {
        public CharacterModel model;
        public CharacterController controller;

        public DataFleeState(EntityModel entityModel)
        {
            model = (CharacterModel)entityModel;
            controller = model.gameObject.GetComponent<CharacterController>();

        }
    }

    public override void EnterState(EntityModel model)
    {
        Debug.Log("FSM NPC Flee START");
        _entitiesData.Add(model, new DataFleeState(model));
        //charModel.View.PlayWalkAnimation(false);
        _entitiesData[model].model.GetRigidbody().velocity = Vector3.zero;
        flee = new Flee(_entitiesData[model].model.transform, _entitiesData[model].controller.CharAIController.Target);
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log("FSM NPC Flee EXECUTE");
        _entitiesData[model].model.LookDir(_entitiesData[model].model.GetComponent<FlockingManager>().RunFlockingDir() + flee.GetDir());
        _entitiesData[model].model.Move(_entitiesData[model].model.GetComponent<FlockingManager>().RunFlockingDir() + flee.GetDir());
    }

    public override void ExitState(EntityModel model)
    {
        Debug.Log("FSM NPC Flee EXIT");
        _entitiesData.Remove(model);
    }
}