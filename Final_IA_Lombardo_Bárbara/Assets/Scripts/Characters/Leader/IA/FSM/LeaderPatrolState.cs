using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UADE.IA.FSM;

public class LeaderPatrolState<T> : FSMState<T>
{
    Leader _leader;
    LeaderAnimations _leaderAnimations;

    FSM<T> _fsm;
    T _idleState;
    T _seekState;

    float patrolCounter;
    float maxPatrolCounter = 10f;

    AgentAStar _agentAstar;

    public LeaderPatrolState(Leader leader, LeaderAnimations leaderAnimations, FSM<T> fsm,
    T idleState, T seekState)
    {
        _leader = leader;
        _leaderAnimations = leaderAnimations;

        _fsm = fsm;
        _idleState = idleState;
        _seekState = seekState;

    }

    public override void Awake()
    {

        Debug.Log("Leader Patrol State Awake");
        patrolCounter = 0f;

        _agentAstar = new AgentAStar(_leader.transform, _leader.mask, _leader.distanceMax, _leader.radius, _leader.offset, _leader.SeekBehaviour, _leader.finit);
    }
    public override void Execute()
    {
        Debug.Log("Leader Patrol State Execute");
        patrolCounter += Time.deltaTime;
        //_leader.Patrolling();

        if (_leader.Line_Of_Sight.targetInSight)
        {
            _fsm.Transition(_seekState);
            Debug.Log("Sight");
        }
        else if (!_leader.Line_Of_Sight.targetInSight && patrolCounter >= maxPatrolCounter)
            _fsm.Transition(_idleState);

    }

    public override void Sleep()
    {
        Debug.Log("Leader Patrol State Sleep");
        patrolCounter = 0;

    }

}
