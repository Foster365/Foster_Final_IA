using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UADE.IA.FSM;

public class LeaderDeathState<T> : FSMState<T>
{

    LeaderAnimations _leaderAnimations;
    public LeaderDeathState(LeaderAnimations leaderAnimations)
    {

        _leaderAnimations = leaderAnimations;
    }

    public override void Awake()
    {
        Debug.Log("Leader DieState Awake");
    }
    public override void Execute()
    {
        Debug.Log("Leader DieState Execute");
        _leaderAnimations.DeathAnimation();
    }
    public override void Sleep()
    {
        Debug.Log("Leader DieState Sleep");
    }

}
