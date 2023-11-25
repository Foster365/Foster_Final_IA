using System.Collections.Generic;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "NPC Attack State", menuName = "Custom SO/FSM States/NPC States/NPC Attack State", order = 0)]
public class NPCAttackState : State
{
    float timer = 0;
    private Dictionary<EntityModel, DataAttackState> _entitiesData = new Dictionary<EntityModel, DataAttackState>();
    private class DataAttackState
    {
        public CharacterModel model;
        public CharacterController controller;
        public List<Node> nodesToTarget;
        public Grid grid;

        public DataAttackState(EntityModel entityModel)
        {
            model = (CharacterModel)entityModel;
            controller = model.gameObject.GetComponent<CharacterController>();
            grid = model.MapGrid;
            nodesToTarget = new List<Node>();
        }
    }

    public override void EnterState(EntityModel model)
    {
        _entitiesData.Add(model, new DataAttackState(model));
        //charModel.View.PlayWalkAnimation(false);
        _entitiesData[model].model.GetRigidbody().velocity = Vector3.zero;
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log("FSM NPC Attack EXECUTE");

        timer += Time.deltaTime;

        if (timer <= _entitiesData[model].model.CharAIData.IdleTimer)
        {
            //if(_entitiesData[model].model.GetComponent<CharacterController>().CharAIController.LineOfSight())_entitiesData[model].model.IsChasing = true;
        }
        else
        {
            timer = 0;
            //_entitiesData[model].model.IsAttack = true;
        }
    }

    public override void ExitState(EntityModel model)
    {
        _entitiesData.Remove(model);
    }
}