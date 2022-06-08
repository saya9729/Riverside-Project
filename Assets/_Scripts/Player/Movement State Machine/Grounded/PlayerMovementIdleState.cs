using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerMovementIdleState : AbstractClass.StateNew
    {
        private PlayerMovementController _playerMovementController;
        public override void EnterState()
        {
            _playerMovementController.SetIdleTargetSpeed();
            _playerMovementController.ResetAirborneDirection();
        }
        public override void ExitState()
        {
            
        }
        protected override void PhysicsUpdateThisState()
        {
            
        }
        public override void SwitchToState(string p_StateType)
        {
            throw new System.NotImplementedException();
        }
        protected override void CheckSwitchState()
        {
            if (_playerMovementController.inputManager.move != Vector2.zero)
            {
                currentSuperState.SwitchToState("Run");
            }
            else if (!_playerMovementController.isGrounded)
            {
                _playerMovementController.ResetAirborneDirection();
                currentSuperState.SwitchToState("IdleWhileAirborne");
            }
        }

        protected override void InitializeComponent()
        {
            _playerMovementController = GetComponent<PlayerMovementController>();
        }
        protected override void InitializeState()
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
