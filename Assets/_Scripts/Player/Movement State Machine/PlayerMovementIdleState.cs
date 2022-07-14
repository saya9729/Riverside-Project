using System.Collections;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerIdleWhileGroundedState))]
    [RequireComponent(typeof(PlayerIdleWhileAirborneState))]
    public class PlayerMovementIdleState : AbstractClass.State
    {
        private PlayerMovementStateManager _playerMovementController;
        
        private PlayerIdleWhileGroundedState _playerIdleWhileGroundedState;
        private PlayerIdleWhileAirborneState _playerIdleWhileAirborneState;
        public override void EnterState()
        {
            try
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
            catch
            {
                Start();
                if (_playerMovementController.isGrounded)
                {
                    SwitchToState("Grounded");
                }
                else
                {
                    SwitchToState("Airborne");
                }
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
                    SetSubState(_playerIdleWhileGroundedState);
                    break;
                case "Airborne":
                    SetSubState(_playerIdleWhileAirborneState);
                    break;
                default:
                    SetSubState(null);
                    break;
            }
        }

        protected override void CheckSwitchState()
        {
            if (_playerMovementController.inputDirection != Vector3.zero)
            {
                currentSuperState.SwitchToState("Run");
            }
            else if (_playerMovementController.IsDashable() && _playerMovementController.inputManager.IsButtonDownThisFrame("Dash"))
            {
                currentSuperState.SwitchToState("Dash");
            }
            else if (_playerMovementController.inputManager.crouch && !_playerMovementController.IsSlideable())
            {
                currentSuperState.SwitchToState("Crouch");
            }
        }

        protected override void InitializeComponent()
        {
            _playerMovementController = GetComponent<PlayerMovementStateManager>();
        }

        protected override void InitializeState()
        {
            _playerIdleWhileGroundedState = GetComponent<PlayerIdleWhileGroundedState>();
            _playerIdleWhileAirborneState = GetComponent<PlayerIdleWhileAirborneState>();
            
            _playerIdleWhileGroundedState.SetSuperState(this);
            _playerIdleWhileAirborneState.SetSuperState(this);
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