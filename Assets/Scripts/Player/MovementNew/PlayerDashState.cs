using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerDashState : AbstractClass.StateNew
    {
        private PlayerMovementController _playerMovementController;        
        public override void EnterState()
        {
            StartCoroutine(WaitAndBackToRun());
            StartCoroutine(StartDashCooldown());
        }
        IEnumerator WaitAndBackToRun()
        {            
            yield return new WaitForSeconds(_playerMovementController.DodgeDuration);
            currentSuperState.SwitchToState("Run");
        }
        IEnumerator StartDashCooldown()
        {
            _playerMovementController._isDashable = false;
            yield return new WaitForSeconds(_playerMovementController.DodgeTimeout);
            _playerMovementController._isDashable = true;
        }

        public override void ExitState()
        {
            StopCoroutine(WaitAndBackToRun());
        }

        protected override void PhysicsUpdateThisState()
        {

        }

        protected override void CheckSwitchState()
        {
            if (_playerMovementController._input.jump)
            {
                currentSuperState.SwitchToState("DashWhileAirborne");                
            }
        }

        protected override void InitializeManager()
        {
            _playerMovementController = GetComponent<PlayerMovementController>();
        }

        protected override void InitializeState()
        {

        }

        protected override void UpdateThisState()
        {
            Dash();
            CheckSwitchState();
        }

        private void Dash()
        {
            _playerMovementController._speed = _playerMovementController.DodgeSpeed;
            _playerMovementController._controller.Move(_playerMovementController._inputDirection.normalized * _playerMovementController._speed * Time.unscaledDeltaTime);
        }

        public override void SwitchToState(string p_StateType)
        {
            throw new System.NotImplementedException();
        }
    }
}