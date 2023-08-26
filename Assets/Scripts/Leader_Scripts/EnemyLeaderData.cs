using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "_main/Data/Enemy Data")]
public class EnemyLeaderData : ScriptableObject
{
    [field: SerializeField] public StateData[] FsmStates { get; private set; }
    [field: SerializeField] public float RestPatrolTime { get; private set; } //necesitamos patrol para el leader
    [field: SerializeField] public float MaxLife { get; private set; }
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float SightRange { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public float DistanceToAttack { get; private set; }
    [field: SerializeField] public float CooldownToAttack { get; private set; }
    [field: SerializeField] public LayerMask TargetLayer { get; private set; }
    [field: SerializeField] public float FleeHealthValue { get; set; }
    [field: SerializeField] public float IdleTimer { get; set; }
}
