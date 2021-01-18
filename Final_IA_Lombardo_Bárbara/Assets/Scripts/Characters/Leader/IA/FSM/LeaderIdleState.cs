using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UADE.IA.FSM;

public class LeaderIdleState<T> : FSMState<T>
{

    Leader _leader;
    LeaderAnimations _leaderAnimations;

    FSM<T> _fsm;
    T _patrolState;
    T _hitStateEnemy;
    T _blockStateEnemy;
    T _dieState;
    T _fleeState;

    float maxCounter = 2f;
    float counter;

    public LeaderIdleState(Leader leader, LeaderAnimations leaderAnimations,
    FSM<T> fsm, T patrolState, T hitState, T dieState, T fleeState)
    {

        _fsm = fsm;
        _patrolState = patrolState;
        _hitStateEnemy = hitState;
        _dieState = dieState;
        _fleeState = fleeState;

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
            _fsm.Transition(_patrolState);
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
        //_leaderAnimations.IdleAnimation();
        //_leader.movementSpeed = 0;// Cuidado
    }
}
