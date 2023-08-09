using UnityEngine;

namespace _Main.Scripts.FSM_SO_VERSION
{
    [CreateAssetMenu(fileName = "FSM State Data", menuName = "Custom SO/FSM State Data", order = 0)]
    public class StateData : ScriptableObject
    {
        [field: SerializeField] public State State { get; private set; }
        [field: SerializeField] public StateCondition[] StateConditions { get; private set; }
        [field: SerializeField] public StateData[] ExitStates { get; private set; }
        
    }
}
