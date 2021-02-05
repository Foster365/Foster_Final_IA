using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UADE.IA.FSM;

public class LeaderIdleState<T> : FSMState<T>
{

    Leader leader;
    LeaderAnimations leaderAnimations;

    FSM<T> fsm;
    T patrolState;
    T hitStateEnemy;
    T blockStateEnemy;
    T dieState;
    T fleeState;

    float maxCounter;
    float counter;

    public LeaderIdleState(Leader _leader, LeaderAnimations _leaderAnimations,
    FSM<T> _fsm, T _patrolState, T _hitState, T _dieState, T _fleeState, float _idleTimer)
    {

        leader = _leader;
        leaderAnimations = _leaderAnimations;

        fsm = _fsm;
        patrolState = _patrolState;
        hitStateEnemy = _hitState;
        dieState = _dieState;
        fleeState = _fleeState;

        maxCounter = _idleTimer;

    }

    public override void Awake()
    {
        Debug.Log("Leader IdleState Awake");
        counter = 0f;
    }

    public override void Execute()
    {
        Debug.Log("Leader IdleState Execute");
        counter += Time.deltaTime;
        IdleBehaviour();
        if (counter >= maxCounter)
            fsm.Transition(patrolState);
        //else if (_leader.CurrentHealth <= 0)
        //    _fsm.Transition(_dieState);


    }

    public override void Sleep()
    {
        Debug.Log("Leader IdleState Sleep");
        counter = 0;
    }

    public void IdleBehaviour()
    {
        leaderAnimations.MoveAnimation(false);
        leaderAnimations.IdleAnimation();
        //leader.movementSpeed = 0;// Cuidado
    }
}
