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

        public DataFollowLeaderState(EntityModel entityModel)
        {
            model = (CharacterModel)entityModel;
            controller = model.gameObject.GetComponent<CharacterController>();
            boss = GameObject.Find(model.Data.BossName);
            nodesToTarget = new List<Node>();
        }
    }

    public override void EnterState(EntityModel model)
    {
        _entitiesData.Add(model, new DataFollowLeaderState(model));
        _entitiesData[model].model.View.CharacterMoveAnimation(true);
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log(_entitiesData[model].model.gameObject.name + "FSM NPC FOLLOW LEADER EXECUTE");

        _entitiesData[model].model.gameObject.GetComponent<CharacterController>().CharAIController.LineOfSight();

        //Diccionario de charactercontroller, que tenga como key el charactermodel. Si existe lo obtengo y si no existe un getcomponent
        //lookup table -> Design pattern

        //_entitiesData[model].model.LookDir(_entitiesData[model].model.GetComponent<FlockingManager>().RunFlockingDir());
        //_entitiesData[model].model.Move(_entitiesData[model].model.GetComponent<FlockingManager>().RunFlockingDir());
        _entitiesData[model].model.gameObject.GetComponent<FlockingManager>().RunFlocking();

        CheckTransitionToIdleState(_entitiesData[model].model);
        CheckTransitionToSeekState(_entitiesData[model].model);

    }

    public override void ExitState(EntityModel model)
    {
        _entitiesData.Remove(model);
    }

    #region Transition to other states

    void CheckTransitionToSeekState(CharacterModel model)
    {
        if (_entitiesData[model].model.gameObject.GetComponent<CharacterController>().CharAIController.Target != null)
        {
            _entitiesData[model].model.IsFollowLeader = false;
            _entitiesData[model].model.IsSeek = true;
        }
    }

    void CheckTransitionToIdleState(CharacterModel model)
    {
        if (_entitiesData[model].boss.gameObject.GetComponent<Rigidbody>().velocity == Vector3.zero)
            _entitiesData[model].model.IsFollowLeader = false;
    }

    #endregion

}