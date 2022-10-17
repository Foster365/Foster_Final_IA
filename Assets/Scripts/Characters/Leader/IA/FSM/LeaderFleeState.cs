using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UADE.IA.FSM;

public class LeaderFleeState<T> : FSMState<T>
{
    Leader _leader;
    LeaderAnimations _leaderAnimations;

    FSM<T> _fsm;
    T _seekState;
    //T _attackState;

    public LeaderFleeState(Leader leader, LeaderAnimations leaderAnimations, FSM<T> fsm,
    T seekState/*, T attackState*/)
    {
        _leader = leader;
        _leaderAnimations = leaderAnimations;

        _fsm = fsm;
        _seekState = seekState;
        //_attackState = attackState;

    }

    public override void Awake()
    {
        Debug.Log("Leader FleeState Awake");
    }

    public override void Execute()
    {

        Debug.Log("Leader Flee Execute");

        _leader.Flee();
        Vector3 safePoint = _leader.CalculateSafePoint();
        if(safePoint!=null)
            //Mover con A*
        //if (_leader.CurrentHealth>=_leader.minimumLife)
        //{
        //    _fsm.Transition(_attackState);
        //    Debug.Log("Transition to Attack");
        //}
        if (_leader.CurrentHealth >= _leader.minimumLife)
        {
            _fsm.Transition(_seekState);
        }

    }

    public override void Sleep()
    {
        Debug.Log("Leader SeekState Sleep");

    }
}
