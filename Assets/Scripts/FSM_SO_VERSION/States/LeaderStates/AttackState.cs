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

    //Regular Attacks roulette wheel
    Roulette _regularAttacksRouletteWheel;
    Dictionary<ActionNode, int> _regularAttacksRouletteWheelNodes = new Dictionary<ActionNode, int>();
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
        //Debug.Log("FSM Leader Attack ENTER");
        if (!_entitiesData.ContainsKey(model)) _entitiesData.Add(model, new DataAttackState(model));
        charModel = _entitiesData[model].model;
        charModel.GetRigidbody().velocity = Vector3.zero;
            attackMaxCooldown = charModel.CharAIData.RegularAttackCooldown;
        attackMaxStateTimer = charModel.CharAIData.RegularAttackStateTimer;
        _regularAttacksRouletteWheelNodes.Clear();
        //attackCooldown = 0;
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log("FSM Leader Attack EXECUTE " + charModel.gameObject.name);
        attackStateTimer += Time.deltaTime;
        var target = _entitiesData[model].controller.CharAIController.Target;
        if (target != null && !target.gameObject.GetComponent<CharacterModel>().HealthController.IsDead)
        {    //Debug.Log("Timer de reg attack: " + attackStateTimer);
            attackCooldown += Time.deltaTime;
            var dist = Vector3.Distance(charModel.transform.position, _entitiesData[model].controller.CharAIController.Target.position);
            if (attackStateTimer < attackMaxStateTimer)
            {
                if (attackCooldown > attackMaxCooldown)
                {
                    RegularAttacksRouletteSetUp();
                    //Debug.Log("timer de reg attack reset");
                    EnemyRegularAttacksRouletteAction();
                    attackCooldown = 0;
                }
                CheckTransitionToSeekState(_entitiesData[model].model, dist);
                CheckTransitionToDeathState(_entitiesData[model].model);
            }
            else
            {
                Debug.Log("Paso a block state?");
                attackStateTimer = 0;
                //charModel.IsAttackDone = true;
                charModel.IsBlocking = true;
                charModel.IsAttacking = false;
            }
        }

    }

    public override void ExitState(EntityModel model)
    {
        //Debug.Log("FSM Leader Attack EXIT");
        _entitiesData.Remove(model);
    }

    void CheckTransitionToSeekState(EntityModel model, float dist)
    {
        if (dist > charModel.Data.AttackRange)
            charModel.IsAttacking = false;
    }
    void CheckTransitionToDeathState(EntityModel model)
    {
        if (charModel.HealthController.CurrentHealth <= 0)
            charModel.IsDead = true;
    }

    #region Regular Attacks Roulette Wheel
    public void RegularAttacksRouletteSetUp()
    {
        _regularAttacksRouletteWheel = new Roulette();

        ActionNode Attack1 = new ActionNode(PlayAttack1);
        ActionNode Attack2 = new ActionNode(PlayAttack2);
        ActionNode Attack3 = new ActionNode(PlayAttack3);
        ActionNode Attack4 = new ActionNode(PlayAttack4);

        _regularAttacksRouletteWheelNodes.Add(Attack1, HandleHealthCondition(charModel.Attack1Chance));
        _regularAttacksRouletteWheelNodes.Add(Attack2, HandleHealthCondition(charModel.Attack2Chance));
        _regularAttacksRouletteWheelNodes.Add(Attack3, HandleHealthCondition(charModel.Attack3Chance));
        _regularAttacksRouletteWheelNodes.Add(Attack4, HandleHealthCondition(charModel.Attack4Chance));
        //_regularAttacksRouletteWheelNodes.Add(Attack4, HandleRouletteOptionModifier(_entitiesData[model].Attack4Chance));

        ActionNode rouletteAction = new ActionNode(EnemyRegularAttacksRouletteAction);
    }

    void PlayAttack1()
    {
        charModel.View.CharacterAttack1Animation();
    }

    void PlayAttack2()
    {
        charModel.View.CharacterAttack2Animation();
    }

    void PlayAttack3()
    {
        charModel.View.CharacterAttack3Animation();
    }

    void PlayAttack4()
    {
        charModel.View.CharacterAttack4Animation();
    }

    public void EnemyRegularAttacksRouletteAction()
    {
        INode node = _regularAttacksRouletteWheel.Run(_regularAttacksRouletteWheelNodes);
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
        if (charModel.HealthController.CurrentHealth < charModel.CharAIData.EnhancedAttackThreshold)
            val += 1;
        else if (charModel.HealthController.CurrentHealth < charModel.CharAIData.EnhancedAttackThreshold)
            val += 2;
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