using UnityEngine;
namespace AbstractClass
{
    public abstract class StateNew : MonoBehaviour
    {
        public StateNew currentSuperState;
        public StateNew currentSubState;

        public virtual void Start()
        {
            InitializeManager();
        }
        public abstract void EnterState();
        protected abstract void UpdateThisState();
        //{
        //    CheckSwitchState();
        //}
        protected abstract void PhysicsUpdateThisState();
        public virtual void PhysicsUpdateAllState()
        {
            PhysicsUpdateThisState();
            if (currentSubState != null)
            {
                currentSubState.PhysicsUpdateAllState();
            }
        }
        public abstract void ExitState();
        protected abstract void CheckSwitchState();
        protected abstract void InitializeState();
        protected abstract void InitializeManager();
        public virtual void UpdateAllState()
        {
            UpdateThisState();
            if (currentSubState != null)
            {
                currentSubState.UpdateAllState();
            }
        }
        public virtual void SetSuperState(StateNew p_newSuperState)
        {
            currentSuperState = p_newSuperState;
        }
        public virtual void SetSubState(StateNew p_newSubState)
        {
            currentSubState.ExitState();
            currentSubState = p_newSubState;
            currentSubState.EnterState();
        }
        public abstract void SwitchToState(string p_StateType);
        //{
        //    switch (p_StateType)
        //    {
        //        case "Null":
        //            SetSubState(null);
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }
}
