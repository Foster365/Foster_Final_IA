using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UADE.IA.FSM
{
    public class FSMStatePRO<T> : FSMState<T>
    {
        public delegate void myDelegate();
        public myDelegate OnAwake;
        public myDelegate OnSleep;
        public myDelegate OnExecute;
        public override void Awake()
        {
            OnAwake();
        }
        public override void Sleep()
        {
            OnSleep();
        }
        public override void Execute()
        {
            OnExecute();
        }
    }
}
