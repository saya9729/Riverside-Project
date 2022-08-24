using UnityEngine;

namespace Player
{
    public class PlayerDashWhileAirborneState : AbstractClass.State
    {
        private PlayerMovementStateManager _playerMovementController;
        public override void EnterState()
        {
            _playerMovementController.SetAirborneInertiaDirection();
            _playerMovementController.DisableStepOffset();
            _playerMovementController.SetDashDirection();
            _playerMovementController.SetAirborneInertiaDirectionWhileDash();
            _playerMovementController.DisableGravity();
            _playerMovementController.StopSpeedChange();
        }
        public override void ExitState()
        {
            _playerMovementController.EnableStepOffset();
            _playerMovementController.EnableRunGravity();
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
            else if (_playerMovementController.CheckLedgeGrab())
            {
                currentSuperState.currentSuperState.SwitchToState("LedgeGrab");
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
            _playerMovementController.MoveWhileDash();
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
