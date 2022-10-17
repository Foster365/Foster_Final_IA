using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UADE.IA.FSM
{
    public class FSMState<T>
    {
        //Funcion de entrada
        public virtual void Awake() { }
        //Funcion de actualizacion del estado
        public virtual void Execute() { }
        //Funcion de salida
        public virtual void Sleep() { }
        //Encapsular los estados con sus respectivos inputs
        Dictionary<T, FSMState<T>> _dic = new Dictionary<T, FSMState<T>>();
        //Agregar estados
        public void AddTransition(T input, FSMState<T> state)
        {
            if (!_dic.ContainsKey(input))
                _dic.Add(input, state);
        }
        //Remover estados
        public void RemoveTransition(T input)
        {
            if (_dic.ContainsKey(input))
                _dic.Remove(input);
        }
        //Obtener estados
        public FSMState<T> GetTransition(T input)
        {
            if (_dic.ContainsKey(input))
            {
                return _dic[input];
            }
            return null;
        }
    }
}
