using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UADE.IA.FSM;

public class LeaderSeekState<T> : FSMState<T>
{
    Leader _leader;
    LeaderAnimations _leaderAnimations;

    FSM<T> _fsm;
    T _patrolState;
    T _attackState;

    public LeaderSeekState(Leader leader, LeaderAnimations leaderAnimations, FSM<T> fsm,
    T patrolState, T attackState)
    {
        _leader = leader;
        _leaderAnimations = leaderAnimations;

        _fsm = fsm;
        _patrolState = patrolState;
        _attackState = attackState;

    }

    public override void Awake()
    {
        Debug.Log("Leader SeekState Awake");
    }

    public override void Execute()
    {

        //_enemyBossAnimations.MoveAnimation(true);

        // var attackDistance = Vector3.Distance(_enemyBoss.transform.position, _target.transform.position);
        _leader.Seek();
        Debug.Log("Leader SeekState Execute");
        if (Vector3.Distance(_leader.transform.position, _leader.Target()) <= _leader.attackRange)
        {
            _fsm.Transition(_attackState);
            Debug.Log("Transition to Attack");
        }
        else if (Vector3.Distance(_leader.transform.position, _leader.Target()) > _leader.attackRange)
        {
            _fsm.Transition(_patrolState);
        }

    }

    public override void Sleep()
    {
        Debug.Log("Leader SeekState Sleep");

    }
}
