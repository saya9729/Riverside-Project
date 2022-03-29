using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbstractClass
{
    public abstract class StateMachineManager : MonoBehaviour
    {
        protected State _currentState;
        public void SwitchState(State p_state)
        {
            _currentState.ExitState();
            _currentState = p_state;
            _currentState.EnterState();
        }
    }
}