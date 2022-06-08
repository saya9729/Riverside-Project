using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerDashWhileAirborneState : AbstractClass.StateNew
    {
        private PlayerMovementController _playerMovementController;
        public override void EnterState()
        {
            DisableStepOffset();
            _playerMovementController.SetAirborneDirection();
            _playerMovementController.SetDashDirection();
            _playerMovementController.DisableGravity();
            _playerMovementController.DisableInput();
            _playerMovementController.ResetInputDirection();
        }        

        private void DisableStepOffset()
        {
            _playerMovementController.DisableStepOffset();
        }
        private void EnableStepOffset()
        {
            _playerMovementController.EnableStepOffset();
        }

        public override void ExitState()
        {
            EnableStepOffset();
            _playerMovementController.EnableGravity();
            _playerMovementController.EnableInput();
        }

        public override void SwitchToState(string p_StateType)
        {
            throw new System.NotImplementedException();
        }

        protected override void CheckSwitchState()
        {
            if (!_playerMovementController.isInDashState)
            {
                _playerMovementController.SetAirborneRunSpeed();
                currentSuperState.SwitchToState("RunWhileAirborne");
            }
            else if (_playerMovementController.isGrounded)
            {
                currentSuperState.SwitchToState("Dash");
            }
        }        

        protected override void InitializeComponent()
        {
            _playerMovementController = GetComponent<PlayerMovementController>();
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
        }

        protected override void InitializeVariable()
        {
            
        }
    }
}
