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
                _playerMovementController.ResetAirborneDirection();
            }
            catch
            {
                Start();
                _playerMovementController.SetRunTargetSpeed();
                _playerMovementController.ResetAirborneDirection();
            }
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
            if (!_playerMovementController.isGrounded)
            {
                _playerMovementController.EnableDoubleJump();
                currentSuperState.SwitchToState("Airborne");
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
