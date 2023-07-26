using UnityEngine;

namespace _Main.Scripts.FSM_SO_VERSION
{
    public abstract class State : ScriptableObject
    {
        public virtual void EnterState(EntityModel model){}
        public abstract void ExecuteState(EntityModel model);
        public virtual void ExitState(EntityModel model){}
    }
    
}

