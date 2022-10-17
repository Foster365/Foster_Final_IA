using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UADE.IA.FSM;

public class LeaderBlockState <T> : FSMState<T>
{
    Leader leader;
    LeaderAnimations leaderAnimations;

    FSM<T> fsm;
    T attackState;

    public LeaderBlockState(Leader _leader, LeaderAnimations _leaderAnimations, FSM<T> _fsm, T _attackState)
    {

    }

    public override void Awake()
    {

        Debug.Log("Leader BlockState Awake");

    }

    public override void Execute()
    {

        Debug.Log("Leader BlockState Execute");

    }

    public override void Sleep()
    {

        Debug.Log("Leader BlockState Sleep");

    }
}
