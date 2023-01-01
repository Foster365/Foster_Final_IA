using System.Collections;
using System.Collections.Generic;
using UADE.IA.FSM;
using UnityEngine;

public class NPCFollowLeaderState<T> : FSMState<T>
{
    //Leader _leader;
    //LeaderAnimations _leaderAnimations;

    NPCAIController npcAIController;

    FSM<T> fsm;
    T idleState;
    T seekState;

    public NPCFollowLeaderState(NPCAIController _npcAIController, FSM<T> _fsm, T _idleState, T _seekState)
    {
        npcAIController = _npcAIController;

        fsm = _fsm;

        idleState = _idleState;
        seekState = _seekState;
    }

    public override void Awake()
    {
        npcAIController.CharModel.ReadyToMove = true;
    }

    public override void Execute()
    {
        npcAIController.FlockBehaviour();

        //if (!npcAIController.LeaderGameObject.gameObject.GetComponent<CharacterModel>().ReadyToMove &&
        //    !npcAIController.CharacterLineOfSight.targetInSight) fsm.Transition(idleState);


        if (!npcAIController.CharacterLineOfSight.targetInSight) fsm.Transition(idleState);

        else if (npcAIController.CharacterLineOfSight.targetInSight &&
            Vector3.Distance(npcAIController.transform.position, npcAIController.CharacterLineOfSight.Target.position)
            < npcAIController.SeekRange) fsm.Transition(seekState);

    }

    public override void Sleep()
    {
    }
}
