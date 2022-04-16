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
            yield return new WaitForSeconds(_playerMovementController.dashDuration);
            currentSuperState.SwitchToState("Run");
        }
        IEnumerator StartDashCooldown()
        {
            _playerMovementController.isDashable = false;
            yield return new WaitForSeconds(_playerMovementController.dashTimeout);
            _playerMovementController.isDashable = true;
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
            if (_playerMovementController.inputManager.jump)
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
            _playerMovementController.speed = _playerMovementController.dashSpeed;
            _playerMovementController.characterController.Move(_playerMovementController.inputDirection.normalized * _playerMovementController.speed * Time.unscaledDeltaTime);
        }

        public override void SwitchToState(string p_StateType)
        {
            throw new System.NotImplementedException();
        }
    }
}