using _Main.Scripts.FSM_SO_VERSION;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Leader Attack State", menuName = "Custom SO/FSM States/Leader States/Leader Attack State", order = 0)]
public class AttackState : State
{
    float attackMaxCooldown, attackCooldown = 0;
    float attackMaxStateTimer, attackStateTimer = 0;

    private Dictionary<EntityModel, DataAttackState> _entitiesData = new Dictionary<EntityModel, DataAttackState>();

    // Attacks roulette wheel
    public Roulette _attacksRouletteWheel;
    public Dictionary<ActionNode, int> _attacksRouletteWheelNodes = new Dictionary<ActionNode, int>();
    public int attack1Chance, attack2Chance, attack3Chance, attack4Chance;
    //

    private class DataAttackState
    {
        public CharacterModel model;
        public CharacterController controller;

        public DataAttackState(EntityModel entityModel)
        {
            model = (CharacterModel)entityModel;
            controller = model.gameObject.GetComponent<CharacterController>();
            //_attacksRouletteWheelNodes.Clear();
        }
    }

    public override void EnterState(EntityModel model)
    {
        if (!_entitiesData.ContainsKey(model)) _entitiesData.Add(model, new DataAttackState(model));
        attackMaxStateTimer = _entitiesData[model].model.CharAIData.AttackStateTimer;
        attackMaxCooldown = _entitiesData[model].model.CharAIData.AttackCooldown;

        attack1Chance = _entitiesData[model].model.Attack1Chance;
        attack2Chance = _entitiesData[model].model.Attack2Chance;
        attack3Chance = _entitiesData[model].model.Attack3Chance;
        attack4Chance = _entitiesData[model].model.Attack4Chance;

        RouletteWheelSetUp(_entitiesData[model].model);

        //attackCooldown = 0;   
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log("FSM Leader Attack EXECUTE " + _entitiesData[model].model.gameObject.name);
        _entitiesData[model].model.HealthController.CanReceiveDamage = true;
        attackStateTimer += Time.deltaTime;
        if (attackStateTimer <= attackMaxStateTimer)
        {

            Debug.Log(_entitiesData[model].model.gameObject.name + "curr health " + _entitiesData[model].model.HealthController.CurrentHealth);

            var target = _entitiesData[model].controller.CharAIController.Target;
            if (target != null)
            {
                var dist = Vector3.Distance(_entitiesData[model].model.transform.position, _entitiesData[model].controller.CharAIController.Target.position);

                var dir = _entitiesData[model].controller.CharAIController.Target.position - _entitiesData[model].model.transform.position;

                _entitiesData[model].model.LookDir(dir);

                attackCooldown += Time.deltaTime;

                if(_attacksRouletteWheelNodes != null) AttackHandler(_entitiesData[model].model);

                CheckTransitionToSeekState(_entitiesData[model].model, dist);
                CheckTransitionToDeathState(_entitiesData[model].model);

            }
        }
        else
        {
            CheckTransitionToBlockState(_entitiesData[model].model);
        }
    }

    public override void ExitState(EntityModel model)
    {
        //if (_attacksRouletteWheelNodes.Count > 0) _attacksRouletteWheelNodes.Clear();
        _entitiesData.Remove(model);
    }

    void AttackHandler(CharacterModel model)
    {
        if (attackCooldown > attackMaxCooldown)
        {
            HandleRWheelAttackChances(_entitiesData[model].model);
            EnemyAttacksRouletteAction();
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

    void CheckTransitionToDeathState(EntityModel model)
    {
        if (_entitiesData[model].model.HealthController.CurrentHealth <= 0)
        {
            Debug.Log("Curr health is zero, transition to death state");
            _entitiesData[model].model.IsDead = true;
            _entitiesData[model].model.IsAttacking = false;
            //_entitiesData[model].model.HealthController.IsDead = true;
            attackCooldown = 0;
            attackStateTimer = 0;
        }
    }

    #endregion

    #region  Attacks Roulette Wheel
    
    public void RouletteWheelSetUp(CharacterModel model)
    {
        Debug.Log("anim ataque seteo ruleta");
        _attacksRouletteWheel = new Roulette();

        ActionNode Attack1 = new ActionNode(_entitiesData[model].model.View.CharacterAttack1Animation);
        ActionNode Attack2 = new ActionNode(_entitiesData[model].model.View.CharacterAttack2Animation);
        ActionNode Attack3 = new ActionNode(_entitiesData[model].model.View.CharacterAttack3Animation);
        ActionNode Attack4 = new ActionNode(_entitiesData[model].model.View.CharacterAttack4Animation);


        _attacksRouletteWheelNodes.Add(Attack1, attack1Chance);
        _attacksRouletteWheelNodes.Add(Attack2, attack2Chance);
        _attacksRouletteWheelNodes.Add(Attack3, attack3Chance);
        _attacksRouletteWheelNodes.Add(Attack4, attack4Chance);

        ActionNode rouletteAction = new ActionNode(EnemyAttacksRouletteAction);
    }

    public void EnemyAttacksRouletteAction()
    {

        INode node = _attacksRouletteWheel.Run(_attacksRouletteWheelNodes);
        node.Execute();
    }

    void HandleRWheelAttackChances(CharacterModel model)
    {
        var currHealth = _entitiesData[model].model.HealthController.CurrentHealth;
        if (currHealth > currHealth * .75f)
        {
            attack1Chance = attack1Chance + 3;
            attack2Chance = attack2Chance + 2;
            attack3Chance = attack3Chance + 1;
            attack4Chance = attack4Chance + 0;
        }
        else if(currHealth < currHealth * .75f && currHealth > currHealth * .35f)
        {
            attack1Chance = attack1Chance + 2;
            attack2Chance = attack2Chance + 1;
            attack3Chance = attack3Chance + 2;
            attack4Chance = attack4Chance + 1;
        }
        else if (currHealth < currHealth * .35f)
        {
            attack1Chance = attack1Chance + 1;
            attack2Chance = attack2Chance + 2;
            attack3Chance = attack3Chance + 3;
            attack4Chance = attack4Chance + 4;
        }
    }

    #endregion
}