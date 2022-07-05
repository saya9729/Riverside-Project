using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerIdleWhileAirborneState : AbstractClass.State
    {
        private PlayerMovementStateManager _playerMovementController;
        public override void EnterState()
        {
            try
            {
                DisableStepOffset();
            }
            catch
            {
                Start();
                DisableStepOffset();
            }
        }

        public override void ExitState()
        {
            EnableStepOffset();
        }

        public override void SwitchToState(string p_StateType)
        {
            throw new System.NotImplementedException();
        }
        private void DisableStepOffset()
        {
            _playerMovementController.DisableStepOffset();
        }
        private void EnableStepOffset()
        {
            _playerMovementController.EnableStepOffset();
        }

        protected override void CheckSwitchState()
        {
            if (_playerMovementController.isGrounded)
            {
                currentSuperState.SwitchToState("Grounded");
            }
        }

        protected override void InitializeComponent()
        {
            _playerMovementController = GetComponent<PlayerMovementStateManager>();
        }

        protected override void InitializeState()
        {
            
        }

        protected override void PhysicsUpdateThisState()
        {
            
        }

        protected override void UpdateThisState()
        {
            CheckSwitchState();
            _playerMovementController.MoveWhileAirborne();
        }

        protected override void InitializeVariable()
        {
            
        }
    }
}
