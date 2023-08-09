using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "AI Data", menuName = "Data/AI Data")]
public class CharacterAIData : ScriptableObject
{

    //FSM variables
    [field: SerializeField] public StateData[] FsmStates { get; private set; }
    [field: SerializeField] public float IdleTimer { get; set; }
    [field: SerializeField] public float PatrolTimer { get; private set; }
    [field: SerializeField] public float TimeForSearchPlayer { get; private set; }
    [field: SerializeField] public LayerMask TargetLayer { get; private set; }

    //Line Of Sight variables
    [field: SerializeField] public LayerMask ObstacleLayerMask { get; private set; }
    [field: SerializeField] public float SightRange { get; private set; }
    [field: SerializeField] public float TotalSightDegrees { get; private set; }

    //Flee variables
    [field: SerializeField] public float FleeThresholdTrigger { get; set; }

    //Obstacle Avoidance variables
    [field: SerializeField] public float ObstacleAvoidanceRadius { get; set; }
    [field: SerializeField] public int ObstacleAvoidanceMaxObstacles { get; set; }
    [field: SerializeField] public float ObstacleAvoidanceViewAngle { get; set; }
    [field: SerializeField] public LayerMask ObstacleAvoidanceLayerMask { get; set; }


}
