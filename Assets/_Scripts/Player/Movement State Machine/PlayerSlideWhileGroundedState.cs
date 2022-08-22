using System.Collections;
using UnityEngine;
using Footsteps;

namespace Player
{
    public class PlayerSlideWhileGroundedState : AbstractClass.State
    {
        private PlayerMovementStateManager _playerMovementController;
        public override void EnterState()
        {
            this.PostEvent(EventID.onPlaySound, AudioID.slide);
            _playerMovementController.GetComponent<CharacterFootsteps>().enabled = false;

            _playerMovementController.SetSlideDirection();
            _playerMovementController.SetSlideTargetSpeed();
            _playerMovementController.EnableSlideGravity();
        }

        public override void ExitState()
        {
            this.PostEvent(EventID.onStopSound, AudioID.slide);
            _playerMovementController.GetComponent<CharacterFootsteps>().enabled = true;

            _playerMovementController.DisableSlideGravity();
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
            _playerMovementController.MoveWhileSlide();
            CheckSwitchState();            
        }
    }
}