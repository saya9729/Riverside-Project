using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AbstractClass
{
    public abstract class StateNew
    {
        protected StateNew _currentSuperState;
        protected StateNew _currentSubState;
        public abstract void EnterState();
        protected abstract void UpdateState();
        public abstract void PhysicsUpdateState();
        public abstract void ExitState();
        protected abstract void CheckSwitchState();
        protected abstract void InitializeSubState();
        public void UpdateAllState()
        {
            UpdateState();
            if (_currentSubState != null)
            {
                _currentSubState.UpdateAllState();
            }
        }
        protected void SetSuperState(StateNew p_newSuperState)
        {
            _currentSuperState = p_newSuperState;
        }
        protected void SetSubState(StateNew p_newSubState)
        {
            _currentSubState = p_newSubState;
            p_newSubState.SetSuperState(this);
        }
    }
}
