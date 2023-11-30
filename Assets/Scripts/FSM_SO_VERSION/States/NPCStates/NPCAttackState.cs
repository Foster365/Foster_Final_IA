using System.Collections.Generic;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "NPC Attack State", menuName = "Custom SO/FSM States/NPC States/NPC Attack State", order = 0)]
public class NPCAttackState : State
{
    float timer = 0;
    float attackMaxCooldown, attackCooldown = 0;
    private Dictionary<EntityModel, DataAttackState> _entitiesData = new Dictionary<EntityModel, DataAttackState>();

    //Regular Attacks roulette wheel
    Roulette _attacksRouletteWheel;
    Dictionary<ActionNode, int> _attacksRouletteWheelNodes = new Dictionary<ActionNode, int>();
    //


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
        _entitiesData.Add(model, new DataAttackState(model));

        attackMaxCooldown = _entitiesData[model].model.CharAIData.AttackCooldown;

        _entitiesData[model].model.GetRigidbody().velocity = Vector3.zero;
        _entitiesData[model].model.View.CharacterMoveAnimation(false);

        AttacksRouletteSetUp(_entitiesData[model].model);
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log(_entitiesData[model].model.gameObject.name + "FSM NPC Attack EXECUTE");


        Debug.Log(_entitiesData[model].model.gameObject.name + " current health is " + _entitiesData[model].model.HealthController.CurrentHealth + " and can receive damage " + _entitiesData[model].model.HealthController.CanReceiveDamage);
        var target = _entitiesData[model].controller.CharAIController.Target;
        if (target != null)
        {
            attackCooldown += Time.deltaTime;
            var dist = Vector3.Distance(_entitiesData[model].model.transform.position, _entitiesData[model].controller.CharAIController.Target.position);
            var dir = _entitiesData[model].controller.CharAIController.Target.position - _entitiesData[model].model.gameObject.transform.position;

            _entitiesData[model].model.LookDir(dir);

            if (attackCooldown > attackMaxCooldown)
            {
                //Debug.Log("timer de reg attack reset");
                EnemyAttacksRouletteAction();
                attackCooldown = 0;
            }

            CheckTransitionToFleeState(_entitiesData[model].model);
            CheckTransitionToDeathState(_entitiesData[model].model);
            CheckTransitionToFollowLeaderState(_entitiesData[model].model);
        }
    }

    public override void ExitState(EntityModel model)
    {
        _entitiesData.Remove(model);
    }

    #region Transition to other states

    public void CheckTransitionToFollowLeaderState(CharacterModel model)
    {
        if (_entitiesData[model].controller.CharAIController.Target == null)
        {
            _entitiesData[model].model.IsAttack = false;
            _entitiesData[model].model.IsFollowLeader = true;
        }
    }

    public void CheckTransitionToFleeState(CharacterModel model)
    {
        if(_entitiesData[model].model.HealthController.CurrentHealth < _entitiesData[model].model.CharAIData.FleeThresholdTrigger)
        {
            _entitiesData[model].model.IsAttack = false;
            _entitiesData[model].model.IsFlee = true;
        }
    }

    public void CheckTransitionToDeathState(CharacterModel model)
    {
        if (_entitiesData[model].model.HealthController.CurrentHealth <= 0)
        {
            Debug.Log(" NPC Curr health is zero, transition to death state");
            _entitiesData[model].model.IsDead = true;
            _entitiesData[model].model.IsAttacking = false;
            //_entitiesData[model].model.HealthController.IsDead = true;
            attackCooldown = 0;
        }
    }

    #endregion

    #region  Attacks Roulette Wheel
    public void AttacksRouletteSetUp(CharacterModel model)
    {
        _attacksRouletteWheel = new Roulette();

        ActionNode Attack1 = new ActionNode(_entitiesData[model].model.View.CharacterAttack1Animation);
        ActionNode Attack2 = new ActionNode(_entitiesData[model].model.View.CharacterAttack2Animation);
        ActionNode Attack3 = new ActionNode(_entitiesData[model].model.View.CharacterAttack3Animation);

        _attacksRouletteWheelNodes.Add(Attack1, _entitiesData[model].model.Attack1Chance);
        _attacksRouletteWheelNodes.Add(Attack2, _entitiesData[model].model.Attack2Chance);
        _attacksRouletteWheelNodes.Add(Attack3, _entitiesData[model].model.Attack3Chance);

        ActionNode rouletteAction = new ActionNode(EnemyAttacksRouletteAction);
    }

    public void EnemyAttacksRouletteAction()
    {
        INode node = _attacksRouletteWheel.Run(_attacksRouletteWheelNodes);
        node.Execute();

    }
    #endregion
}