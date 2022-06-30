using System.Collections;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerCrouchWhileGroundedState))]
    [RequireComponent(typeof(PlayerCrouchWhileAirborneState))]
    public class PlayerCrouchState : AbstractClass.State
    {
        private PlayerMovementStateManager _playerMovementController;

        private PlayerCrouchWhileGroundedState _playerCrouchWhileGroundedState;
        private PlayerCrouchWhileAirborneState _playerCrouchWhileAirborneState;
        public override void EnterState()
        {
            if (_playerMovementController.isGrounded)
            {
                SwitchToState("Grounded");
            }
            else
            {
                SwitchToState("Airborne");
            }
        }

        public override void ExitState()
        {

        }

        public override void SwitchToState(string p_stateType)
        {
            switch (p_stateType)
            {
                case "Grounded":
                    SetSubState(_playerCrouchWhileGroundedState);
                    break;
                case "Airborne":
                    SetSubState(_playerCrouchWhileAirborneState);
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
                currentSuperState.SwitchToState("Run");
            }
        }

        protected override void InitializeComponent()
        {
            _playerMovementController = GetComponent<PlayerMovementStateManager>();
        }

        protected override void InitializeState()
        {
            _playerCrouchWhileGroundedState = GetComponent<PlayerCrouchWhileGroundedState>();
            _playerCrouchWhileAirborneState = GetComponent<PlayerCrouchWhileAirborneState>();

            _playerCrouchWhileGroundedState.SetSuperState(this);
            _playerCrouchWhileAirborneState.SetSuperState(this);
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