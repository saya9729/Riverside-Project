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
            _playerMovementController.SetDashSpeed();
            _playerMovementController.SetAirborneDirection();
            _playerMovementController.SetDashDirection();
            _playerMovementController.DisableGravity();
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
        }

        public override void SwitchToState(string p_StateType)
        {
            throw new System.NotImplementedException();
        }

        protected override void CheckSwitchState()
        {
            if (_playerMovementController.isGrounded)
            {
                currentSuperState.SwitchToState("Dash");
            }
            else if (!_playerMovementController.isInDashState)
            {
                _playerMovementController.SetAirborneRunSpeed();
                currentSuperState.SwitchToState("RunWhileAirborne");
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
