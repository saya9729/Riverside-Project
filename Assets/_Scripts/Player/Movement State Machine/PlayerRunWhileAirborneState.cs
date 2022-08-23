using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerRunWhileAirborneState : AbstractClass.State
    {
        private PlayerMovementStateManager _playerMovementController;
        public override void EnterState()
        {
            _playerMovementController.SetAirborneInertiaDirection();
            try
            {
                _playerMovementController.DisableStepOffset();
            }
            catch
            {
                Start();
                _playerMovementController.DisableStepOffset();
            }
            _playerMovementController.StopSpeedChange();
        }

        public override void ExitState()
        {
            _playerMovementController.EnableStepOffset();
        }

        public override void SwitchToState(string p_StateType)
        {
            throw new System.NotImplementedException();
        }

        protected override void CheckSwitchState()
        {
            if (_playerMovementController.isGrounded)
            {
                currentSuperState.SwitchToState("Grounded");
            }
            else if (_playerMovementController.CheckLedgeGrab())
            {
                currentSuperState.currentSuperState.SwitchToState("LedgeGrab");
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
            _playerMovementController.MoveWhileAirborne();
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
