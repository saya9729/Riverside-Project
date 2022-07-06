using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerSlideWhileGroundedState))]
    [RequireComponent(typeof(PlayerSlideWhileAirborneState))]
    public class PlayerSlideState : AbstractClass.State
    {
        private PlayerMovementStateManager _playerMovementController;

        private PlayerSlideWhileGroundedState _playerSlideWhileGroundedState;
        private PlayerSlideWhileAirborneState _playerSlideWhileAirborneState;
        public override void EnterState()
        {
            _playerMovementController.SetSlideDirection();
            SwitchToState("Grounded");
            _playerMovementController.StartCoroutineCrouchDown();
        }

        public override void ExitState()
        {
            
        }

        public override void SwitchToState(string p_stateType)
        {
            switch (p_stateType)
            {
                case "Grounded":
                    SetSubState(_playerSlideWhileGroundedState);
                    break;
                case "Airborne":
                    SetSubState(_playerSlideWhileAirborneState);
                    break;
                default:
                    SetSubState(null);
                    break;
            }
        }

        protected override void CheckSwitchState()
        {
            if (!_playerMovementController.inputManager.crouch)
            {
                _playerMovementController.StarCoroutineStandUp();
                currentSuperState.SwitchToState("Run");
            }
            else if (_playerMovementController.IsDashable() && _playerMovementController.inputManager.dash)
            {
                _playerMovementController.StarCoroutineStandUp();
                currentSuperState.SwitchToState("Dash");
            }
        }

        protected override void InitializeComponent()
        {
            _playerMovementController = GetComponent<PlayerMovementStateManager>();
        }

        protected override void InitializeState()
        {
            _playerSlideWhileGroundedState = GetComponent<PlayerSlideWhileGroundedState>();
            _playerSlideWhileAirborneState = GetComponent<PlayerSlideWhileAirborneState>();

            _playerSlideWhileGroundedState.SetSuperState(this);
            _playerSlideWhileAirborneState.SetSuperState(this);
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