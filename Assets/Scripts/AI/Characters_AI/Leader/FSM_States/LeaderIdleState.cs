using System;
using System.Collections;
using System.Collections.Generic;
using UADE.IA.FSM;
using UnityEngine;

public class LeaderIdleState<T> : FSMState<T>
{
    //Leader _leader;
    //LeaderAnimations _leaderAnimations;

    LeaderAIController leaderAIController;

    float idleMaxTimer, idleTimer;

    FSM<T> _fsm;
    T patrolState;
    T seekState;
    T attackState;
    T blockState;
    T damageState;
    T deathState;

    public LeaderIdleState(LeaderAIController _leaderAIController, float _idleMaxTimer,
        FSM<T> fsm, T _patrolState, T _seekState, T _attackState, T _blockState, T _damageState, T _deathState)
    {
        leaderAIController = _leaderAIController;
        idleMaxTimer = _idleMaxTimer;

        _fsm = fsm;

        patrolState = _patrolState;
        seekState = _seekState;
        attackState = _attackState;
        blockState = _blockState;
        damageState = _damageState;
        deathState = _deathState;
    }

    public override void Awake()
    {
        idleTimer = 0;
    }

    public override void Execute()
    {
        Debug.Log("Leader Idle State Execute");
        idleTimer += Time.deltaTime;
        leaderAIController.CharModel.ReadyToMove = false;
        ////Debug.Log("TIMER: " + idleTimer);
        leaderAIController.CharacterLineOfSight.GetLineOfSight();
        if (idleTimer >= idleMaxTimer)
        {
            _fsm.Transition(patrolState);
        }
        else if (leaderAIController.CharacterLineOfSight.Target != null)
        {
            //if (Vector3.Distance(leaderAIController.transform.position,
            //    leaderAIController.CharacterLineOfSight.Target.position) <= leaderAIController.SeekRange)
            //{
            //    _fsm.Transition(seekState);
            //}
            //if (Vector3.Distance(leaderAIController.transform.position,
            //    leaderAIController.CharacterLineOfSight.Target.position)
            //    < leaderAIController.AttackRange)
            //{
            //    _fsm.Transition(attackState);
            //}
        }
        else if (leaderAIController.CharModel.CharacterHealthController.IsDamage) _fsm.Transition(damageState);
        else if (leaderAIController.CharModel.CharacterHealthController.IsDead) _fsm.Transition(deathState);
        //else _fsm.Transition(seekState); //En realidad sí va el seeek, hay que volver a setearlo en el constructor y el controller
    }

    public override void Sleep()
    {
    }
}
