using _Main.Scripts.FSM_SO_VERSION;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity Data", menuName = "Data/Entity Data")]
public class EntityData : ScriptableObject
{
    //Basic Entity variables
    [field: SerializeField] public int MovementSpeed { get; private set; }
    [field: SerializeField] public float DistanceToAttack { get; private set; }
    [field: SerializeField] public float CooldownToAttack { get; private set; }
    [field: SerializeField] public float RotationSpeed { get; private set; }

    //FSM variables
    [field: SerializeField] public StateData[] FsmStates { get; private set; }
    [field: SerializeField] public float IdleTimer { get; set; }
    [field: SerializeField] public float RestPatrolTime { get; private set; }
    [field: SerializeField] public float TimeForSearchPlayer { get; private set; }
    [field: SerializeField] public LayerMask TargetLayer { get; private set; }
}