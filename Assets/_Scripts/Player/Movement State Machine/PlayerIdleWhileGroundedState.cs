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
            }
            catch
            {
                Start();
                _playerMovementController.SetIdleTargetSpeed();
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
                _playerMovementController.EnableDoubleJump();
                _playerMovementController.SetAirborneInertiaDirection();
                _playerMovementController.StopSpeedChange();
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
            _playerMovementController.MoveWhileGrounded();
            CheckSwitchState();
        }

        protected override void InitializeVariable()
        {
            
        }
    }
}
