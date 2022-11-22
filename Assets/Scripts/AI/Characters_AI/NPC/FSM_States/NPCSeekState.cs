using System.Collections;
using System.Collections.Generic;
using UADE.IA.FSM;
using UnityEngine;

public class NPCSeekState<T> : FSMState<T>
{
    //Leader _leader;
    //LeaderAnimations _leaderAnimations;

    NPCAIController npcAIController;

    FSM<T> fsm;
    T followLeaderState;
    T attackState;

    public NPCSeekState(NPCAIController _npcAIController, FSM<T> _fsm, T _followLeaderState, T _attackState)
    {
        npcAIController = _npcAIController;

        fsm = _fsm;

        followLeaderState = _followLeaderState;
        attackState = _attackState;
    }

    public override void Awake()
    {

        npcAIController.NpcPursuitSteeringBehaviour = new Pursuit(npcAIController.transform, npcAIController.PursuitSBTimePrediction);
    }

    public override void Execute()
    {
        Debug.Log("NPC Seek State Execute");
        //npcAIController.CharModel.SeekBehaviour();

        SeekBehaviour();

        if (npcAIController.CharacterLineOfSight.targetInSight &&
            Vector3.Distance(npcAIController.transform.position, npcAIController.CharacterLineOfSight.Target.position)
            > npcAIController.SeekRange) fsm.Transition(followLeaderState);

        else if (npcAIController.CharacterLineOfSight.targetInSight &&
            Vector3.Distance(npcAIController.transform.position, npcAIController.CharacterLineOfSight.Target.position)
            < npcAIController.AttackRange) fsm.Transition(attackState);

    }

    public override void Sleep()
    {
    }

    void SeekBehaviour()
    {
        Vector3 dir = npcAIController.NpcPursuitSteeringBehaviour.GetDir(npcAIController.CharacterLineOfSight.Target,
            npcAIController.CharacterLineOfSight.Target.gameObject.GetComponent<Rigidbody>());
        npcAIController.CharModel.Move(dir.normalized);
    }

}
