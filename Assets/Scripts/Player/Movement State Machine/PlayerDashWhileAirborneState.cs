using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerDashWhileAirborneState : AbstractClass.StateNew
    {
        private PlayerMovementController _playerMovementController;
        public override void EnterState()
        {
            StartCoroutine(WaitAndBackToRunWhileAirborne());
            StartCoroutine(StartDashCooldown());
        }
        IEnumerator WaitAndBackToRunWhileAirborne()
        {
            yield return new WaitForSeconds(_playerMovementController.dashDuration);
            currentSuperState.SwitchToState("RunWhileAirborne");
        }
        IEnumerator StartDashCooldown()
        {
            _playerMovementController.isDashable = false;
            yield return new WaitForSeconds(_playerMovementController.dashTimeout);
            _playerMovementController.isDashable = true;
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
            if (_playerMovementController.isGrounded)
            {
                currentSuperState.SwitchToState("Dash");
            }
        }
        private void Dash()
        {
            _playerMovementController.speed = _playerMovementController.dashSpeed;
            _playerMovementController.characterController.Move(_playerMovementController.inputDirection.normalized * _playerMovementController.speed * Time.unscaledDeltaTime);
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
            Dash();
            CheckSwitchState();
        }
    }
}
