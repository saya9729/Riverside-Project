using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerRunState : AbstractClass.StateNew
    {
        private PlayerMovementController _playerMovementController;
        public override void EnterState()
        {
            //play run animation
            _playerMovementController.SetRunTargetSpeed();
            _playerMovementController.ResetAirborneDirection();
        }

        public override void ExitState()
        {
            //stop run animation
        }

        public override void SwitchToState(string p_StateType)
        {
            throw new System.NotImplementedException();
        }

        protected override void CheckSwitchState()
        {
            if (_playerMovementController.inputManager.move == Vector2.zero)
            {
                currentSuperState.SwitchToState("Idle");
            }
            else if (!_playerMovementController.isGrounded)
            {
                currentSuperState.SwitchToState("RunWhileAirborne");
            }
            else if (_playerMovementController.isDashable && _playerMovementController.inputManager.dash)
            {
                _playerMovementController.StartCoroutineDashState();
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

        protected override void InitializeVariable()
        {
            
        }

        protected override void PhysicsUpdateThisState()
        {
            
        }

        protected override void UpdateThisState()
        {
            CheckSwitchState();
        }
    }
}
