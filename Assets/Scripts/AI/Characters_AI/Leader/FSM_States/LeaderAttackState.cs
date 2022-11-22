using System.Collections;
using System.Collections.Generic;
using UADE.IA.FSM;
using UnityEngine;

public class LeaderAttackState<T> : FSMState<T>
{
    //Leader _leader;
    //LeaderAnimations _leaderAnimations;

    LeaderAIController leaderAIController;

    float cooldownTimer = 0;

    FSM<T> fsm;
    T seekState;
    T damageState;
    T idleState;

    public LeaderAttackState(LeaderAIController _leaderAIController, FSM<T> _fsm,
    T _seekState, T _damageState, T _idleState)
    {

        leaderAIController = _leaderAIController;

        fsm = _fsm;
        seekState = _seekState;
        damageState = _damageState;
        idleState = _idleState;
    }

    public override void Awake()
    {
        cooldownTimer = 0;
    }

    public override void Execute()
    {
        if (leaderAIController.CharacterLineOfSight.Target != null)
        {

            //leaderAIController.CharModel.LookAtTarget(leaderAIController.CharacterLineOfSight.Target.position);

            cooldownTimer += Time.deltaTime;

            if (cooldownTimer >= leaderAIController.AttackCooldown &&
                Vector3.Distance(leaderAIController.transform.position, leaderAIController.CharacterLineOfSight.Target.position)
                < leaderAIController.AttackRange)
            {
                leaderAIController.AttackRouletteAction();
                fsm.Transition(idleState);
            }
            else if (Vector3.Distance(leaderAIController.transform.position, leaderAIController.CharacterLineOfSight.Target.position)
                >= leaderAIController.AttackRange)
            {
                cooldownTimer = 0;
                fsm.Transition(seekState);
            }
            else if (leaderAIController.CharModel.CharacterHealthController.IsDamage)
            {
                cooldownTimer = 0;
                fsm.Transition(damageState);
            }

        }

    }

    public override void Sleep()
    {
        cooldownTimer = 0;
    }
}
