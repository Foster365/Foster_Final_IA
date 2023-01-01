using System.Collections;
using System.Collections.Generic;
using UADE.IA.FSM;
using UnityEngine;

public class NPCFleeState<T> : FSMState<T>
{
    NPCAIController npcAIController;

    FSM<T> fsm;
    T idleState;
    T deathState;

    public NPCFleeState(NPCAIController _npcAIController, FSM<T> _fsm, T _idleState, T _deathState)
    {
        npcAIController = _npcAIController;

        fsm = _fsm;

        idleState = _idleState;
        deathState = _deathState;
    }
    public override void Awake()
    {
        //npcAIController.NpcFleeSteeringBehaviour = new Flee(npcAIController.CharModel.MovementSpeed, npcAIController.CharModel.rotationSpeed,
        //    npcAIController.transform, npcAIController.CharacterLineOfSight, npcAIController.CharacterLineOfSight.Target,
        //    GetDirFromLineOfSightTarget(npcAIController.CharacterLineOfSight.Target));

    }
    public override void Execute()
    {
        Debug.Log("NPC Flee State Execute");
        //if (npcAIController.CharacterLineOfSight.Target != null) FleeBehaviour();
    }

    void FleeBehaviour()
    {
        npcAIController.NpcFleeSteeringBehaviour.GetDir();
    }
    Vector3 GetDirFromLineOfSightTarget(Transform _target)
    {
        return _target.position - npcAIController.transform.position;
    }

}
