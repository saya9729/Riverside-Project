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
            if (_playerMovementController.particleDash)
            {
                _playerMovementController.RotateWindParticle();
                this.PostEvent(EventID.onPlayVFX, VFXID.dash);
            }

            _playerMovementController.StartCoroutineChangeFOVWhileSlide();
            _playerMovementController.StartCoroutineSlideState();

            SwitchToState("Grounded");
            _playerMovementController.StartCoroutineCrouchDown();
            _playerMovementController.animator.SetBool("isSlide",true);
        }

        public override void ExitState()
        {
            if (_playerMovementController.particleDash)
            {
                _playerMovementController.ResetWindParticleRotation();
                this.PostEvent(EventID.onStopVFX, VFXID.dash);
            }

            _playerMovementController.StarCoroutineRevertFOVAfterSlide();
            _playerMovementController.animator.SetBool("isSlide", false);
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
            if (!_playerMovementController.isRoofed)
            {
                if (!_playerMovementController.inputManager.crouch)
                {
                    _playerMovementController.StarCoroutineStandUp();
                    currentSuperState.SwitchToState("Run");
                }
                else if (_playerMovementController.IsDashable() && _playerMovementController.inputManager.IsButtonDownThisFrame("Dash"))
                {
                    _playerMovementController.StarCoroutineStandUp();
                    currentSuperState.SwitchToState("Dash");
                }
            }
            else
            {

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