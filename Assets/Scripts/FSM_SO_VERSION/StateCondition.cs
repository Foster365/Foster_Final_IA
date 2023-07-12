using UnityEngine;

namespace _Main.Scripts.FSM_SO_VERSION
{

    public abstract class StateCondition : ScriptableObject
    {
        public abstract bool CompleteCondition(EntityModel model);
    }
}