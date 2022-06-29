using UnityEngine;

namespace Player
{
    public class PlayerDashWhileAirborneState : AbstractClass.State
    {
        private PlayerMovementStateManager _playerMovementController;
        public override void EnterState()
        {
            DisableStepOffset();
            _playerMovementController.SetDashDirection();
            _playerMovementController.SetAirborneInertiaDirectionWhileDash();
            _playerMovementController.DisableGravity();
        }

        private void DisableStepOffset()
        {
            _playerMovementController.DisableStepOffset();
        }
        private void EnableStepOffset()
        {
            _playerMovementController.EnableStepOffset();
        }

        public override void ExitState()
        {
            EnableStepOffset();
            _playerMovementController.EnableGravity();
        }

        public override void SwitchToState(string p_StateType)
        {
            throw new System.NotImplementedException();
        }

        protected override void CheckSwitchState()
        {
            if (!_playerMovementController.isInDashState)
            {
                currentSuperState.currentSuperState.SwitchToState("Run");
            }
            else if (_playerMovementController.isGrounded)
            {
                currentSuperState.SwitchToState("Grounded");
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
