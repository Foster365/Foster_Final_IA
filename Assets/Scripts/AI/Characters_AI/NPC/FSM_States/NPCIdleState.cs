using System.Collections;
using System.Collections.Generic;
using UADE.IA.FSM;
using UnityEngine;

public class NPCIdleState<T> : FSMState<T>
{
    //Leader _leader;
    //LeaderAnimations _leaderAnimations;

    NPCAIController npcAIController;

    Vector3 flockingTarget;

    FSM<T> fsm;
    T followLeaderState;
    T seekState;
    T attackState;
    T fleeState;
    T damageState;
    T deathState;

    public NPCIdleState(NPCAIController _npcAIController, FSM<T> _fsm,
        T _followLeaderState, T _seekState, T _attackState, T _damageState, T _fleeState, T _deathState)
    {
        npcAIController = _npcAIController;

        fsm = _fsm;

        followLeaderState = _followLeaderState;
        seekState = _seekState;
        attackState = _attackState;
        fleeState = _fleeState;
        damageState = _damageState;
        deathState = _deathState;
    }

    public override void Awake()
    {
        npcAIController.CharModel.ReadyToMove = false;
    }

    public override void Execute()
    {
        Debug.Log("NPC Idle State Execute");
        npcAIController.CharacterLineOfSight.GetLineOfSight();

        //if (npcAIController.LeaderGameObject.gameObject.GetComponent<CharacterModel>().ReadyToMove &&
        //    !npcAIController.CharacterLineOfSight.targetInSight) fsm.Transition(followLeaderState);
        if (!npcAIController.CharacterLineOfSight.targetInSight) fsm.Transition(followLeaderState);
        else if (npcAIController.CharacterLineOfSight.targetInSight &&
            Vector3.Distance(npcAIController.transform.position, npcAIController.CharacterLineOfSight.Target.position)
            < npcAIController.SeekRange) fsm.Transition(seekState);

        else if (npcAIController.CharacterLineOfSight.targetInSight &&
            Vector3.Distance(npcAIController.transform.position, npcAIController.CharacterLineOfSight.Target.position)
            < npcAIController.AttackRange) fsm.Transition(attackState);

        else if (npcAIController.CharModel.CharacterHealthController.CurrentHealth < 15) fsm.Transition(fleeState);

        else if (npcAIController.CharModel.CharacterHealthController.IsDamage) fsm.Transition(damageState);

        else if (npcAIController.CharModel.CharacterHealthController.IsDead
            || npcAIController.LeaderGameObject.GetComponent<HealthController>().IsDead) fsm.Transition(deathState);

    }
    public override void Sleep()
    {
    }
}
