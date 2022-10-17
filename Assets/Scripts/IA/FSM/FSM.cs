using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UADE.IA.FSM
{
    public class FSM<T>
    {
        //Encapsular el estado actual
        FSMState<T> _current;
        //Aca inicializamos la FSM con un estado inicial
        public void SetInit(FSMState<T> init)
        {
            _current = init;
            _current.Awake();
        }
        //Actualizar el estado
        public void OnUpdate()
        {
            if (_current != null)
                _current.Execute();
        }
        //Realizamos la transicion

        public void Transition(T input)
        {
            //Obtenemos el estado a transicionar
            FSMState<T> newState = _current.GetTransition(input);
            //Si no existe ese estado, o si no tengo conexion a el(desde el estado acutal) no realizo la transicion 
            if (newState == null) return;
            //Realizamos la funcion de la salida del estado actual
            _current.Sleep();
            //Sobreescribimos el estado actual por el nuevo
            _current = newState;
            //Reproducimos la funcion de entrada del nuevo estado
            _current.Awake();
        }
    }
}