using _Main.Scripts.FSM_SO_VERSION;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Leader Regular Attack State", menuName = "Custom SO/FSM States/Leader States/Leader Regular Attack State", order = 0)]
public class RegularAttackState : State
{
    float attackMaxCooldown = 2f, attackCooldown = 0;
    float attackMaxStateTimer = 5f, attackStateTimer = 0;
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
        Debug.Log("FSM Leader Attack ENTER");
        if (!_entitiesData.ContainsKey(model)) _entitiesData.Add(model, new DataAttackState(model));
        _entitiesData[model].model.GetRigidbody().velocity = Vector3.zero;
        //attackCooldown = 0;
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log("FSM Leader Attack EXECUTE");
        attackStateTimer += Time.deltaTime;
        if(attackStateTimer < attackMaxStateTimer)
        {
            attackCooldown += Time.deltaTime;
            if(attackCooldown > attackMaxCooldown)
            {
                Debug.Log("Attack cooldown timer: " + attackCooldown);
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
        Debug.Log("FSM Leader Attack EXIT");
        _entitiesData.Remove(model);
    }
}