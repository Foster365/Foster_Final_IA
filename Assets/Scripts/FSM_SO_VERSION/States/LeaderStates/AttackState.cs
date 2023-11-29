using _Main.Scripts.FSM_SO_VERSION;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Leader Attack State", menuName = "Custom SO/FSM States/Leader States/Leader Attack State", order = 0)]
public class AttackState : State
{
    float attackMaxCooldown, attackCooldown = 0;
    float attackMaxStateTimer, attackStateTimer = 0;

    CharacterModel charModel;
    private Dictionary<EntityModel, DataAttackState> _entitiesData = new Dictionary<EntityModel, DataAttackState>();

    // Attacks roulette wheel
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
        if (!_entitiesData.ContainsKey(model)) _entitiesData.Add(model, new DataAttackState(model));
        charModel = _entitiesData[model].model;
            attackMaxCooldown = charModel.CharAIData.AttackCooldown;
        attackMaxStateTimer = charModel.CharAIData.AttackStateTimer;
        //attackCooldown = 0;
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log("FSM Leader Attack EXECUTE " + charModel.gameObject.name);
        _entitiesData[model].model.HealthController.CanReceiveDamage = true;
        attackStateTimer += Time.deltaTime;
        if (attackStateTimer <= attackMaxStateTimer)
        {
            var target = _entitiesData[model].controller.CharAIController.Target;
            if (!target.gameObject.GetComponent<CharacterModel>().HealthController.IsDead)
            {

                var dist = Vector3.Distance(_entitiesData[model].model.transform.position, _entitiesData[model].controller.CharAIController.Target.position);

                var dir = _entitiesData[model].controller.CharAIController.Target.position - _entitiesData[model].model.transform.position;

                _entitiesData[model].model.LookDir(dir);

                attackCooldown += Time.deltaTime;

                CheckTransitionToSeekState(_entitiesData[model].model, dist);
                CheckTransitionToDeathState(_entitiesData[model].model);


                AttackHandler(_entitiesData[model].model);
            }
        }
        else
        {
            CheckTransitionToBlockState(_entitiesData[model].model);
        }
    }

    public override void ExitState(EntityModel model)
    {
        _entitiesData.Remove(model);
    }

    void AttackHandler(CharacterModel model)
    {
        if (attackCooldown > attackMaxCooldown)
        {
            _entitiesData[model].model.View.CharacterAttack1Animation();
            //_attacksRouletteWheelNodes.Clear();
            //AttacksRouletteSetUp(_entitiesData[model].model);
            //EnemyAttacksRouletteAction();
            attackCooldown = 0;
        }
    }

    #region Transitions to other states

    void CheckTransitionToSeekState(EntityModel model, float dist)
    {
        if (dist > _entitiesData[model].model.Data.AttackRange)
        {
            _entitiesData[model].model.IsAttacking = false;
            attackCooldown = 0;
            attackStateTimer = 0;
        }
    }
    void CheckTransitionToDeathState(EntityModel model)
    {
        if (_entitiesData[model].model.HealthController.CurrentHealth <= 0)
        {
            _entitiesData[model].model.IsDead = true;
            attackCooldown = 0;
            attackStateTimer = 0;
        }
    }
    void CheckTransitionToBlockState(CharacterModel model)
    {
        if (attackStateTimer > attackMaxStateTimer)
        {
            _entitiesData[model].model.IsBlocking = true;
            _entitiesData[model].model.IsAttacking = false;
            attackCooldown = 0;
            attackStateTimer = 0;
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
        ActionNode Attack4 = new ActionNode(_entitiesData[model].model.View.CharacterAttack4Animation);

        _attacksRouletteWheelNodes.Add(Attack1,_entitiesData[model].model.Attack1Chance);
        _attacksRouletteWheelNodes.Add(Attack2,_entitiesData[model].model.Attack2Chance);
        _attacksRouletteWheelNodes.Add(Attack3,_entitiesData[model].model.Attack3Chance);
        _attacksRouletteWheelNodes.Add(Attack4,_entitiesData[model].model.Attack4Chance);

        ActionNode rouletteAction = new ActionNode(EnemyAttacksRouletteAction);
    }

    public void EnemyAttacksRouletteAction()
    {
        INode node = _attacksRouletteWheel.Run(_attacksRouletteWheelNodes);
        node.Execute();

    }

    int HandleRouletteOptionModifier(int value)
    {
        var finalValue = value;
        if (charModel.gameObject.CompareTag(TagManager.LEADER_TAG)) finalValue = HandleBossRouletteOptionsModifier(value);
        else finalValue = HandleNPCRouletteOptionsModifier(value);

        return finalValue;
    }
    int HandleHealthCondition(int val)
    {
        //Debug.Log("Events for health attack enhancement value is: " + val);
        //if (charModel.HealthController.CurrentHealth < charModel.CharAIData.EnhancedAttackThreshold)
        //    val += 1;
        //else if (charModel.HealthController.CurrentHealth < charModel.CharAIData.EnhancedAttackThreshold)
        //    val += 2;
        //Debug.Log("Events for health attack enhancement final value is: " + val);
        return val;
    }
    public int HandleBossRouletteOptionsModifier(int value)
    {
        //value = OnHealthCondition?.Invoke(value) ?? value;
        return value;
    }

    public int HandleNPCRouletteOptionsModifier(int value)
    {
        return value;
    }

    #endregion
}