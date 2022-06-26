using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerIdleWhileGroundedState : AbstractClass.State
    {
        private PlayerMovementStateManager _playerMovementController;
        public override void EnterState()
        {
            try
            {
                _playerMovementController.SetIdleTargetSpeed();
                _playerMovementController.ResetAirborneDirection();
            }
            catch
            {
                Start();
                _playerMovementController.SetIdleTargetSpeed();
                _playerMovementController.ResetAirborneDirection();
            }
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
            if (!_playerMovementController.isGrounded)
            {
                _playerMovementController.ResetAirborneDirection();
                _playerMovementController.EnableDoubleJump();
                currentSuperState.SwitchToState("Airborne");
            }
        }

        protected override void InitializeComponent()
        {
            _playerMovementController = GetComponent<PlayerMovementStateManager>();
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
