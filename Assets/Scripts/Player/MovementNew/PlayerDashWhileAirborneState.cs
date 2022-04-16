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
            yield return new WaitForSeconds(_playerMovementController.DodgeDuration);
            currentSuperState.SwitchToState("RunWhileAirborne");
        }
        IEnumerator StartDashCooldown()
        {
            _playerMovementController._isDashable = false;
            yield return new WaitForSeconds(_playerMovementController.DodgeTimeout);
            _playerMovementController._isDashable = true;
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
            if (_playerMovementController.Grounded)
            {
                currentSuperState.SwitchToState("Dash");
            }
        }
        private void Dash()
        {
            _playerMovementController._speed = _playerMovementController.DodgeSpeed;
            _playerMovementController._controller.Move(_playerMovementController._inputDirection.normalized * _playerMovementController._speed * Time.unscaledDeltaTime);
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
