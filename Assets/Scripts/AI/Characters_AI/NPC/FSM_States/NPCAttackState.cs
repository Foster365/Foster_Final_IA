using System;
using System.Collections;
using System.Collections.Generic;
using UADE.IA.FSM;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NPCAttackState<T> : FSMState<T>
{
    //Leader _leader;
    //LeaderAnimations _leaderAnimations;

    NPCAIController npcAIController;
    float cooldownTimer;
    FSM<T> fsm;
    T seekState;
    T idleState;
    T damageState;

    public NPCAttackState(NPCAIController _npcAIController, FSM<T> _fsm, T _idleState, T _damageState)
    {
        npcAIController = _npcAIController;

        fsm = _fsm;

        //seekState = _seekState;
        idleState = _idleState;
        damageState = _damageState;
    }

    public override void Awake()
    {
        cooldownTimer = 0;
    }

    public override void Execute()
    {
        Debug.Log("NPC Attack State Execute");

        if (npcAIController.CharacterLineOfSight.Target != null)
        {

            //npcAIController.CharModel.LookAtTarget(npcAIController.CharacterLineOfSight.Target.position);

            cooldownTimer += Time.deltaTime;

            if (cooldownTimer >= npcAIController.AttackCooldown &&
                Vector3.Distance(npcAIController.transform.position, npcAIController.CharacterLineOfSight.Target.position)
                < npcAIController.AttackRange)
            {
                npcAIController.AttackRouletteAction();
                fsm.Transition(idleState);
            }
            //else if (Vector3.Distance(npcAIController.transform.position, npcAIController.CharacterLineOfSight.Target.position)
            //    >= npcAIController.AttackRange)
            //{
            //    cooldownTimer = 0;
            //    fsm.Transition(seekState);
            //}
            else if (npcAIController.CharModel.CharacterHealthController.IsDamage)
            {
                cooldownTimer = 0;
                fsm.Transition(damageState);
            }

        }

    }

    public override void Sleep()
    {
        cooldownTimer = 0;
    }
}
