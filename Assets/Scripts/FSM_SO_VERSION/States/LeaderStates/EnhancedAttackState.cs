using _Main.Scripts.FSM_SO_VERSION;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Leader Enhanced Attack State", menuName = "Custom SO/FSM States/Leader States/Leader Enhanced Attack State", order = 0)]
public class EnhancedAttackState : State
{
    float attackMaxCooldown = 5f, attackCooldown = 0;
    float attackMaxStateTimer = 10f, attackStateTimer = 0;
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
        Debug.Log("FSM Leader Enhanced Attack ENTER");
        if (!_entitiesData.ContainsKey(model)) _entitiesData.Add(model, new DataAttackState(model));
        _entitiesData[model].model.GetRigidbody().velocity = Vector3.zero;
        attackMaxCooldown = _entitiesData[model].model.CharAIData.EnhancedAttackCooldown;
        attackMaxStateTimer = _entitiesData[model].model.CharAIData.EnhancedAttackStateTimer;
        //attackCooldown = 0;
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log("FSM Leader Enhanced Attack EXECUTE");
        attackStateTimer += Time.deltaTime;
        attackCooldown += Time.deltaTime;
        if (attackStateTimer < attackMaxStateTimer)
        {
            Debug.Log("Timer de reg attack: " + attackCooldown);
            if (attackCooldown > attackMaxCooldown)
            {
                Debug.Log("timer de reg attack reset");
                _entitiesData[model].controller.CharAIController.EnemyRegularAttacksRouletteAction();
                attackCooldown = 0;
            }
        }
        else
        {
            attackStateTimer = 0;
        }

    }

    public override void ExitState(EntityModel model)
    {
        Debug.Log("FSM Leader Enhanced Attack EXIT");
        _entitiesData.Remove(model);
    }
}