using UnityEngine;
namespace AbstractClass
{
    public abstract class State : MonoBehaviour
    {
        public State currentSuperState;
        public State currentSubState;

        public virtual void Start()
        {            
            InitializeComponent();
            InitializeVariable();
            InitializeState();
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
        protected abstract void InitializeComponent();
        protected abstract void InitializeVariable();
        public virtual void UpdateAllState()
        {
            UpdateThisState();
            if (currentSubState != null)
            {
                currentSubState.UpdateAllState();
            }
        }
        public virtual void SetSuperState(State p_newSuperState)
        {
            currentSuperState = p_newSuperState;
        }
        public virtual void SetSubState(State p_newSubState)
        {
            currentSubState.ExitState();
            currentSubState = p_newSubState;
            currentSubState.EnterState();
        }
        public abstract void SwitchToState(string p_stateType);
        //{
        //    switch (p_stateType)
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
