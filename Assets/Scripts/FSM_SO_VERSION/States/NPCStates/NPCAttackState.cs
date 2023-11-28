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
    CharacterModel charModel;


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
        _entitiesData[model].model.GetRigidbody().velocity = Vector3.zero;
        _entitiesData[model].model.View.CharacterMoveAnimation(false);
        attackMaxCooldown = _entitiesData[model].model.CharAIData.AttackCooldown;
        charModel = _entitiesData[model].model;
        AttacksRouletteSetUp(_entitiesData[model].model);
    }

    public override void ExecuteState(EntityModel model)
    {
        Debug.Log(_entitiesData[model].model.gameObject.name + "FSM NPC Attack EXECUTE");

        timer += Time.deltaTime;
        var target = _entitiesData[model].controller.CharAIController.Target;
        if (target != null)
        {
            if (!target.gameObject.GetComponent<CharacterModel>().HealthController.IsDead)
            {    //Debug.Log("Timer de reg attack: " + attackStateTimer);
                attackCooldown += Time.deltaTime;
                var dist = Vector3.Distance(_entitiesData[model].model.transform.position, _entitiesData[model].controller.CharAIController.Target.position);
                if (attackCooldown > attackMaxCooldown)
                {
                    //Debug.Log("timer de reg attack reset");
                    EnemyAttacksRouletteAction();
                    attackCooldown = 0;
                }
                CheckTransitionToFollowLeaderState(_entitiesData[model].model);
                CheckTransitionToFleeState(_entitiesData[model].model);
                CheckTransitionToDeathState(_entitiesData[model].model);
            }
        }
    }

    public override void ExitState(EntityModel model)
    {
        _entitiesData.Remove(model);
    }

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
        if(_entitiesData[model].model.HealthController.CurrentHealth <= _entitiesData[model].model.CharAIData.FleeThresholdTrigger)
        {
            _entitiesData[model].model.IsAttack = false;
            _entitiesData[model].model.IsFlee = true;
        }
    }

    public void CheckTransitionToDeathState(CharacterModel model)
    {

        if (_entitiesData[model].model.HealthController.CurrentHealth <= 0)
        {
            _entitiesData[model].model.IsAttack = false;
            _entitiesData[model].model.HealthController.IsDead = true;
        }
    }

    #region  Attacks Roulette Wheel
    public void AttacksRouletteSetUp(CharacterModel model)
    {
        _attacksRouletteWheel = new Roulette();

        ActionNode Attack1 = new ActionNode(PlayAttack1);
        ActionNode Attack2 = new ActionNode(PlayAttack2);
        ActionNode Attack3 = new ActionNode(PlayAttack3);

        _attacksRouletteWheelNodes.Add(Attack1, 10);
        _attacksRouletteWheelNodes.Add(Attack2, 15);
        _attacksRouletteWheelNodes.Add(Attack3, 20);

        ActionNode rouletteAction = new ActionNode(EnemyAttacksRouletteAction);
    }

    void PlayAttack1()
    {
        //Debug.Log("FSM NPC Attack " + charModel);
        charModel.View.CharacterAttack1Animation();
    }

    void PlayAttack2()
    {
        //Debug.Log("FSM NPC Attack " + charModel);
        charModel.View.CharacterAttack2Animation();
    }

    void PlayAttack3()
    {
        //Debug.Log("FSM NPC Attack " + charModel);
        charModel.View.CharacterAttack3Animation();
    }


    public void EnemyAttacksRouletteAction()
    {
        INode node = _attacksRouletteWheel.Run(_attacksRouletteWheelNodes);
        node.Execute();

    }
    #endregion
}