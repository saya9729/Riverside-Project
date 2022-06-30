using UnityEngine;
namespace Player
{
    public class PlayerRunWhileGroundedState : AbstractClass.State
    {
        private PlayerMovementStateManager _playerMovementController;
        public override void EnterState()
        {
            try
            {
                _playerMovementController.SetRunTargetSpeed();
            }
            catch
            {
                Start();
                _playerMovementController.SetRunTargetSpeed();
            }
        }

        public override void ExitState()
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
            else if (_playerMovementController.inputManager.crouch && _playerMovementController.IsSlideable())
            {
                currentSuperState.currentSuperState.SwitchToState("Slide");
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
        }
    }
}
