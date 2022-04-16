using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerRunState : AbstractClass.StateNew
    {
        private PlayerMovementController _playerMovementController;
        public override void EnterState()
        {
            //play run animation
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
            if (_playerMovementController._input.move == Vector2.zero)
            {
                currentSuperState.SwitchToState("Idle");
            }
            else if (!_playerMovementController.Grounded)
            {
                currentSuperState.SwitchToState("RunWhileAirborne");
            }
            else if (_playerMovementController._isDashable && _playerMovementController._input.dash)
            {
                currentSuperState.SwitchToState("Dash");
            }
        }

        protected override void InitializeManager()
        {
            _playerMovementController = GetComponent<PlayerMovementController>();
        }

        protected override void InitializeState()
        {
            throw new System.NotImplementedException();
        }

        protected override void PhysicsUpdateThisState()
        {
            
        }

        protected override void UpdateThisState()
        {
            Move();
            CheckSwitchState();
        }

        private void Move()
        {
            _playerMovementController._speed = _playerMovementController.RunSpeed;
            // move the player
            _playerMovementController._controller.Move(_playerMovementController._inputDirection.normalized * _playerMovementController._speed * Time.unscaledDeltaTime + new Vector3(0.0f, _playerMovementController._verticalVelocity, 0.0f) * Time.unscaledDeltaTime);
            //_playerRigidbody.AddForce(inputDirection.normalized * (_speed * Time.unscaledDeltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.unscaledDeltaTime);
        }
    }
}
