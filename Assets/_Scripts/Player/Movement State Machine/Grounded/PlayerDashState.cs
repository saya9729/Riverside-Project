using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerDashState : AbstractClass.StateNew
    {
        private PlayerMovementController _playerMovementController;
        public GameObject particleDash;

        public override void EnterState()
        {
            StartCoroutine(WaitAndBackToRun());
            StartCoroutine(StartDashCooldown());
        }
        IEnumerator WaitAndBackToRun()
        {
            if (particleDash)
            {
                if (!particleDash.activeSelf)
                {
                    particleDash.SetActive(true);
                }
            }

            yield return new WaitForSeconds(_playerMovementController.dashDuration * Time.timeScale);

            if (particleDash)
            {
                if (particleDash.activeSelf)
                {
                    particleDash.SetActive(false);
                }
            }

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

        protected override void InitializeComponent()
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

        protected override void InitializeVariable()
        {

        }
    }
}