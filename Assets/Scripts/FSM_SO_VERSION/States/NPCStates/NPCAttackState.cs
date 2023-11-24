using System.Collections.Generic;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "NPC Attack State", menuName = "Custom SO/FSM States/NPC States/NPC Attack State", order = 0)]
public class NPCAttackState : State
{
    float attackMaxCooldown, attackCooldown = 0;
    private Dictionary<EntityModel, DataAttackState> _entitiesData = new Dictionary<EntityModel, DataAttackState>();
    private class DataAttackState
    {
        public CharacterModel model;
        public CharacterController controller;

        public DataAttackState(EntityModel entityModel)
        {
            model = (CharacterModel)entityModel;
            controller = model.gameObject.GetComponent<CharacterController>();

        }
    }

    public override void EnterState(EntityModel model)
    {
        Debug.Log("FSM NPC Attack ENTER");
        if (!_entitiesData.ContainsKey(model)) _entitiesData.Add(model, new DataAttackState(model));
        _entitiesData[model].model.GetRigidbody().velocity = Vector3.zero;
        attackMaxCooldown = _entitiesData[model].model.CharAIData.RegularAttackCooldown;
        //attackCooldown = 0;
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log("FSM NPC Attack EXECUTE " + _entitiesData[model].model.gameObject.name);
        var target = _entitiesData[model].controller.CharAIController.Target;
        if (target != null && !target.gameObject.GetComponent<CharacterModel>().HealthController.IsDead)
        {    //Debug.Log("Timer de reg attack: " + attackStateTimer);
            attackCooldown += Time.deltaTime;
            var dist = Vector3.Distance(_entitiesData[model].model.transform.position, _entitiesData[model].controller.CharAIController.Target.position);
            if (attackCooldown > attackMaxCooldown)      
            { 
                //Debug.Log("timer de reg attack reset"); 
                _entitiesData[model].controller.CharAIController.EnemyRegularAttacksRouletteAction(); //el npc tiene roulette?  
                attackCooldown = 0;
            }
                CheckTransitionToSeekState(_entitiesData[model].model, dist);
                CheckTransitionToDeathState(_entitiesData[model].model);
        }

    }

    public override void ExitState(EntityModel model)
    {
        Debug.Log("FSM NPC Attack EXIT");
        _entitiesData.Remove(model);
    }

    void CheckTransitionToSeekState(EntityModel model, float dist)
    {
        if (dist > _entitiesData[model].model.Data.AttackRange)
            _entitiesData[model].model.IsAttacking = false;
    }
    void CheckTransitionToDeathState(EntityModel model)
    {
        if (_entitiesData[model].model.HealthController.CurrentHealth <= 0)
            _entitiesData[model].model.IsDead = true;
    }
}