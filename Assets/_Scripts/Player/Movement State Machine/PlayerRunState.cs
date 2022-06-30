using System.Collections;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerRunWhileGroundedState))]
    [RequireComponent(typeof(PlayerRunWhileAirborneState))]
    public class PlayerRunState : AbstractClass.State
    {
        private PlayerMovementStateManager _playerMovementController;
        
        private PlayerRunWhileGroundedState _playerRunWhileGroundedState;
        private PlayerRunWhileAirborneState _playerRunWhileAirborneState;
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
                    SetSubState(_playerRunWhileGroundedState);
                    break;
                case "Airborne":
                    SetSubState(_playerRunWhileAirborneState);
                    break;
                default:
                    SetSubState(null);
                    break;
            }
        }

        protected override void CheckSwitchState()
        {
            if (_playerMovementController.inputManager.move == Vector2.zero)
            {
                currentSuperState.SwitchToState("Idle");
            }
            else if (_playerMovementController.IsDashable() && _playerMovementController.inputManager.dash)
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
            _playerRunWhileGroundedState = GetComponent<PlayerRunWhileGroundedState>();
            _playerRunWhileAirborneState = GetComponent<PlayerRunWhileAirborneState>();
            
            _playerRunWhileGroundedState.SetSuperState(this);
            _playerRunWhileAirborneState.SetSuperState(this);
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