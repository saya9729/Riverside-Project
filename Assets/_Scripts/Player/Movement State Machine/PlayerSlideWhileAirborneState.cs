using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerSlideWhileAirborneState : AbstractClass.State
    {
        private PlayerMovementStateManager _playerMovementController;
        public override void EnterState()
        {
            _playerMovementController.SetAirborneInertiaDirection();
            _playerMovementController.DisableStepOffset();
            //_playerMovementController.StopSpeedChange(); not meant to be call because of the slide jump speed boost
        }

        public override void ExitState()
        {
            _playerMovementController.EnableStepOffset();
        }

        public override void SwitchToState(string p_stateType)
        {

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

        protected override void InitializeVariable()
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
    }
}