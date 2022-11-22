using System.Collections;
using System.Collections.Generic;
using UADE.IA.FSM;
using UnityEngine;

public class NPCDeathState<T> : FSMState<T>
{
    //Leader _leader;
    //LeaderAnimations _leaderAnimations;

    NPCAIController npcAIController;

    FSM<T> fsm;

    public NPCDeathState(NPCAIController _npcAIController, FSM<T> _fsm)
    {
        npcAIController = _npcAIController;
        fsm = _fsm;
    }

    public override void Awake()
    {
    }

    public override void Execute()
    {
        Debug.Log("NPC Death State Execute");
        npcAIController.CharModel.CharacterAnimController.CharacterDeathAnimation();
    }

    public override void Sleep()
    {
    }
}
