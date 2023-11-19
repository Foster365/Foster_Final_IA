using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "AI Data", menuName = "Data/AI Data")]
public class CharacterAIData : ScriptableObject
{

    //FSM variables
    [field: SerializeField, Header("FSM Variables")] public StateData[] FsmStates { get; private set; }
    [field: SerializeField] public float IdleTimer { get; set; }
    [field: SerializeField] public float PatrolTimer { get; private set; }
    [field: SerializeField] public float BlockStateTimer { get; private set; }
    [field: SerializeField] public float RegularAttackStateTimer { get; private set; }
    [field: SerializeField] public float EnhancedAttackStateTimer { get; private set; }
    [field: SerializeField] public float RegularAttackCooldown { get; private set; }
    [field: SerializeField] public float EnhancedAttackCooldown { get; private set; }
    [field: SerializeField] public float DesperateAttackCooldown { get; private set; }
    [field: SerializeField] public float EnhancedAttackThreshold { get; private set; }
    [field: SerializeField] public float DesperateAttackThreshold { get; private set; }
    [field: SerializeField] public float TimeForSearchPlayer { get; private set; }
    [field: SerializeField] public LayerMask TargetLayer { get; private set; }
    [field: SerializeField] public float RandomPositionThreshold { get; private set; }

    //Line Of Sight variables
    [field: SerializeField, Header("Line Of Sight Variables")] public LayerMask ObstacleLayerMask { get; private set; }
    [field: SerializeField] public float SightRange { get; private set; }
    [field: SerializeField] public float TotalSightDegrees { get; private set; }

    //Flee variables
    [field: SerializeField, Header("Flee SB Variables")] public float FleeThresholdTrigger { get; set; }

    //Obstacle Avoidance variables
    [field: SerializeField, Header("Obstacle Avoidance SB Variables")] public float ObstacleAvoidanceRadius { get; set; }
    [field: SerializeField] public int ObstacleAvoidanceMaxObstacles { get; set; }
    [field: SerializeField] public float ObstacleAvoidanceViewAngle { get; set; }
    [field: SerializeField] public LayerMask ObstacleAvoidanceLayerMask { get; set; }


}
