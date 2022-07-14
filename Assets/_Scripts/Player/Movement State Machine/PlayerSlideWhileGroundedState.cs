using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerSlideWhileGroundedState : AbstractClass.State
    {
        private PlayerMovementStateManager _playerMovementController;
        public override void EnterState()
        {
            _playerMovementController.SetSlideDirection();
            _playerMovementController.SetSlideTargetSpeed();
            _playerMovementController.EnableSlideGravity();
            _playerMovementController.StartCoroutineSlideState();
        }

        public override void ExitState()
        {
            _playerMovementController.DisableSlideGravity();
            _playerMovementController.StopCoroutineSlideState();
        }

        public override void SwitchToState(string p_stateType)
        {

        }

        protected override void CheckSwitchState()
        {
            if (!_playerMovementController.isGrounded)
            {
                _playerMovementController.EnableDoubleJump();
                _playerMovementController.SetAirborneInertiaDirection();
                //_playerMovementController.StopSpeedChange(); useless because set target speed later                
                _playerMovementController.SetSlideJumpTargetSpeed();
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

        protected override void InitializeVariable()
        {

        }

        protected override void PhysicsUpdateThisState()
        {

        }

        protected override void UpdateThisState()
        {
            CheckSwitchState();
            _playerMovementController.MoveWhileSlide();
        }
    }
}