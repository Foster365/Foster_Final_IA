using System.Collections;
using System.Collections.Generic;
using UADE.IA.FSM;
using UnityEngine;

public class NPCDamageState<T> : FSMState<T>
{
    //Leader _leader;
    //LeaderAnimations _leaderAnimations;

    NPCAIController npcAIController;

    FSM<T> fsm;
    T idleState;
    public NPCDamageState(NPCAIController _npcAIController, FSM<T> _fsm, T _idleState)
    {
        npcAIController = _npcAIController;

        fsm = _fsm;

        idleState = _idleState;
    }

    public override void Awake()
    {
    }

    public override void Execute()
    {
        Debug.Log("NPC Damage State Execute");

        if (npcAIController.CharModel.CharacterHealthController.IsDamage)
        {
            npcAIController.CharModel.CharacterAnimController.CharacterDamageAnimation();
            fsm.Transition(idleState);
        }
    }

    public override void Sleep()
    {
    }
}
