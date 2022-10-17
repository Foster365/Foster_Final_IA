using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UADE.IA.FSM;

public class LeaderDamagedState<T> : FSMState<T>
{
    Leader _leader;
    LeaderAnimations _leaderAnimations;

    FSM<T> _fsm;
    T _attackState;
    T _idleState;
    T _blockStateEnemy;

    public LeaderDamagedState(Leader leader, LeaderAnimations leaderAnimations, FSM<T> fsm,
    T attackState, T idleState, T blockStateEnemy)
    {

        _leader = leader;
        _leaderAnimations = leaderAnimations;

        _fsm = fsm;
        _attackState = attackState;
        _idleState = idleState;
        _blockStateEnemy = blockStateEnemy;

    }

    public override void Awake()
    {
        Debug.Log("Leader Damaged State Awake");
    }

    public override void Execute()
    {
        Debug.Log("Leader Damaged State Execute");
        _leaderAnimations.DamageAnimation();


    }

    public override void Sleep()
    {
        Debug.Log("Leader Damage State Sleep");
    }
}
