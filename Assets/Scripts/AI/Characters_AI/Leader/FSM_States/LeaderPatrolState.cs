using UADE.IA.FSM;
using UnityEngine;

public class LeaderPatrolState<T> : FSMState<T>
{
    //Leader _leader;
    //LeaderAnimations _leaderAnimations;

    LeaderAIController leaderAIController;

    Transform target;

    float patrolMaxTimer, patrolTimer;

    FSM<T> fsm;
    T idleState;
    T seekState;

    public LeaderPatrolState(LeaderAIController _leaderAIController, float _patrolMaxTimer, FSM<T> _fsm, T _idleState, T _seekState)
    {
        leaderAIController = _leaderAIController;

        patrolMaxTimer = _patrolMaxTimer;

        patrolTimer = 0;
        fsm = _fsm;

        idleState = _idleState;
        seekState = _seekState;
    }

    public override void Awake()
    {
        patrolTimer = 0;
    }

    public override void Execute()
    {
        Debug.Log("Leader Patrol State Execute");
        PatrolBehaviour();
        patrolTimer += Time.deltaTime;

        if (leaderAIController.CharacterLineOfSight.targetInSight) fsm.Transition(seekState);
        else if (!leaderAIController.CharacterLineOfSight.targetInSight && patrolTimer >= patrolMaxTimer) fsm.Transition(idleState);

    }

    public override void Sleep()
    {
        patrolTimer = 0;
    }

    void PatrolBehaviour()
    {
        leaderAIController.CharModel.ReadyToMove = true;
        //leaderAIController.CharModel.PatrolSimple();//Patrol(leaderAIController.CharacterPathfinding);
        leaderAIController.CharacterLineOfSight.GetLineOfSight();

    }
}
