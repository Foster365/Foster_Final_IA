using System.Collections;
using System.Collections.Generic;
using UADE.IA.FSM;
using UnityEngine;

public class LeaderDeathState<T> : FSMState<T>
{
    //Leader _leader;
    //LeaderAnimations _leaderAnimations;

    LeaderAIController leaderAIController;

    FSM<T> _fsm;

    public LeaderDeathState(LeaderAIController _leaderAIController, FSM<T> fsm)
    {
        leaderAIController = _leaderAIController;
        _fsm = fsm;
    }

    public override void Awake()
    {
    }

    public override void Execute()
    {
        Debug.Log("Leader Death State Execute");
        leaderAIController.CharModel.CharacterHealthController.Die();
    }

    public override void Sleep()
    {
    }
}
