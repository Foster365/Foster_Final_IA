using System.Collections.Generic;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "Follow Leader State", menuName = "Custom SO/FSM States/NPC States/Follow Leader State", order = 0)]
public class NPCFollowLeaderState : State
{
    float timer = 0;
    private Dictionary<EntityModel, DataFollowLeaderState> _entitiesData = new Dictionary<EntityModel, DataFollowLeaderState>();
    Flee npcFlee;

    GameObject boss;
    private class DataFollowLeaderState
    {
        public CharacterModel model;
        public CharacterController controller;
        public GameObject boss;
        public List<Node> nodesToTarget;
        public Grid grid;

        public DataFollowLeaderState(EntityModel entityModel)
        {
            model = (CharacterModel)entityModel;
            controller = model.gameObject.GetComponent<CharacterController>();
            boss = GameObject.Find(model.Data.BossName);
            grid = model.MapGrid;
            nodesToTarget = new List<Node>();
        }
    }

    public override void EnterState(EntityModel model)
    {
        _entitiesData.Add(model, new DataFollowLeaderState(model));
        //charModel.View.PlayWalkAnimation(false);
        _entitiesData[model].model.GetRigidbody().velocity = Vector3.zero;
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log("FSM NPC FOLLOW LEADER EXECUTE");

        _entitiesData[model].model.gameObject.GetComponent<CharacterController>().CharAIController.LineOfSight();

        _entitiesData[model].model.LookDir(_entitiesData[model].model.GetComponent<FlockingManager>().RunFlockingDir());
        _entitiesData[model].model.Move(_entitiesData[model].model.GetComponent<FlockingManager>().RunFlockingDir());

        //CheckTarget(_entitiesData[model]);
        if (model.gameObject.GetComponent<CharacterController>().CharAIController.Target != null)
        {
            model.IsFollowLeader = false;
            model.IsSeek = true;
        }
        else if (_entitiesData[model].boss.gameObject.GetComponent<Rigidbody>().velocity == Vector3.zero) _entitiesData[model].model.IsFollowLeader = false;
        //else if (_entitiesData[model].gameObject.GetComponent<CharacterController>().CharAIController.IsTargetInSight)
        //    _entitiesData[model].IsSeek = true;
    }

    public override void ExitState(EntityModel model)
    {
        _entitiesData.Remove(model);
    }

    void CheckTarget(EntityModel model)
    {
        if(model.gameObject.GetComponent<CharacterController>().CharAIController.Target != null)
        {
            model.IsFollowLeader = false;
            model.IsSeek = true;
        }
    }
}