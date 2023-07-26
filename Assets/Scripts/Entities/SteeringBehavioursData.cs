using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

[CreateAssetMenu(fileName = "SB Data", menuName = "Data/Steering Behaviours Data")]
public class SteeringBehavioursData : ScriptableObject
{

    //Pursuit variables
    [field: SerializeField] public float PursuitTime { get; private set; }

    //Line Of Sight variables
    [field: SerializeField] public float SightRange { get; private set; }
    [field: SerializeField] public float TotalSightDegrees { get; private set; }

    //Flee variables
    [field: SerializeField] public float FleeHealthValue { get; set; }


}
