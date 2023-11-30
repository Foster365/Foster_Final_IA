using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "NPC Idle State", menuName = "Custom SO/FSM States/NPC States/NPC Idle State", order = 0)]
public class NPCIdleState : State
{
    float timer = 0;
    private Dictionary<EntityModel, DataIdleState> _entitiesData = new Dictionary<EntityModel, DataIdleState>();
    GameObject boss;
    private class DataIdleState
    {
        public CharacterModel model;
        public CharacterController controller;
        public GameObject boss;

        public DataIdleState(EntityModel entityModel)
        {
            model = (CharacterModel)entityModel;
            controller = model.gameObject.GetComponent<CharacterController>();
            boss = GameObject.Find(model.Data.BossName);
        }
    }

    public override void EnterState(EntityModel model)
    {
        _entitiesData.Add(model, new DataIdleState(model));
        _entitiesData[model].model.HealthController.CanReceiveDamage = true;
        //charModel.View.PlayWalkAnimation(false);
        _entitiesData[model].model.GetRigidbody().velocity = Vector3.zero;
        _entitiesData[model].model.View.CharacterMoveAnimation(false);
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log(_entitiesData[model].model.gameObject.name + "FSM NPC IDLE EXECUTE");

        timer += Time.deltaTime;
        if (_entitiesData[model].boss.gameObject.GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            _entitiesData[model].model.IsFollowLeader = true;
        }
        else if (_entitiesData[model].boss.gameObject.GetComponent<CharacterModel>().IsBattleBegun)
            _entitiesData[model].model.IsSearching = true;
    }

    public override void ExitState(EntityModel model)
    {
        _entitiesData.Remove(model);
    }
}