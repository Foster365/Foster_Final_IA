using System;
using System.Collections;
using System.Collections.Generic;
using UADE.IA.FSM;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LeaderBlockState<T> : FSMState<T>
{
    //Leader _leader;
    //LeaderAnimations _leaderAnimations;

    LeaderAIController leaderAIController;
    float cooldownTimer = 0;

    FSM<T> fsm;
    T idleState;

    public LeaderBlockState(LeaderAIController _leaderAIController, FSM<T> _fsm, T _idleState)
    {
        leaderAIController = _leaderAIController;
        fsm = _fsm;
        idleState = _idleState;
    }

    public override void Awake()
    {
        cooldownTimer = 0;
    }

    public override void Execute()
    {
        //Debug.Log("Leader Block State Execute");
        //cooldownTimer += Time.deltaTime;
        //leaderAIController.BlockRouletteAction();
        //if (cooldownTimer >= leaderAIController.BlockCooldown)
        //{
        //    leaderAIController.IdleAttackTransitionRouletteAction();
        //    //cooldownTimer = 0;
        //    //fsm.Transition(idleState);
        //}
    }

    public override void Sleep()
    {
        cooldownTimer = 0;
    }
}
