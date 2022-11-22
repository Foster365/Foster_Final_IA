using UADE.IA.FSM;
using UnityEngine;

public class LeaderSeekState<T> : FSMState<T>
{
    //Leader _leader;
    //LeaderAnimations _leaderAnimations;

    LeaderAIController leaderAIController;

    float attackRange;

    FSM<T> _fsm;
    T _patrolState;
    T _attackState;

    public LeaderSeekState(LeaderAIController _leaderAIController, float _attackRange, FSM<T> fsm,
    T patrolState, T attackState)
    {

        leaderAIController = _leaderAIController;

        attackRange = _attackRange;

        _fsm = fsm;
        _patrolState = patrolState;
        _attackState = attackState;
    }

    public override void Awake()
    {
        leaderAIController.CharModel.ReadyToMove = true;
    }

    public override void Execute()
    {
        Debug.Log("Leader Seek State Execute");

        SeekBehaviour();
        if (Vector3.Distance(leaderAIController.transform.position,
            leaderAIController.CharacterLineOfSight.Target.position) <= attackRange)
            _fsm.Transition(_attackState);

        else if (Vector3.Distance(leaderAIController.transform.position,
            leaderAIController.CharacterLineOfSight.Target.position) > leaderAIController.SeekRange) _fsm.Transition(_patrolState);

    }

    public override void Sleep()
    {
        leaderAIController.CharModel.ReadyToMove = false;
    }

    void SeekBehaviour()
    {
        leaderAIController.CharacterPathfinding.FindPath(leaderAIController.transform.position,
            leaderAIController.CharacterLineOfSight.Target.position);
        leaderAIController.CharModel.Run(leaderAIController.CharacterPathfinding.finalPath);
    }

}
