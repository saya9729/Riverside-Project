using System.Collections;
using UnityEngine;

namespace AbstractClass
{
    public abstract class StateMachineManagerNew : MonoBehaviour
    {
        protected StateNew _currentState;
        protected abstract void InitializeState();
        protected void SwitchState(StateNew p_state)
        {
            _currentState.ExitState();
            _currentState = p_state;
            _currentState.EnterState();
        }
    }
}