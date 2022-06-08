using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerIdleWhileAirborneState : AbstractClass.StateNew
    {
        private PlayerMovementController _playerMovementController;
        public override void EnterState()
        {
            //start animation
            _playerMovementController.SetIdleTargetSpeed();
            DisableStepOffset();
            _playerMovementController.SetAirborneDirection();
        }

        public override void ExitState()
        {
            //stop animation
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
                currentSuperState.SwitchToState("Idle");
            }
            else if (_playerMovementController.inputManager.move!=Vector2.zero)
            {
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
