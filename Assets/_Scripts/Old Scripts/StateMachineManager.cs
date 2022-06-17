using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbstractClass
{
    public abstract class StateMachineManager : MonoBehaviour
    {
        protected StateOld _currentState;
        public void SwitchState(StateOld p_state)
        {
            _currentState.ExitState();
            _currentState = p_state;
            _currentState.EnterState();
        }
    }
}