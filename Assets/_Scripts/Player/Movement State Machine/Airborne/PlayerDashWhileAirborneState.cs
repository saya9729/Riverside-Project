using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerDashWhileAirborneState : AbstractClass.StateNew
    {
        private PlayerMovementController _playerMovementController;
        public GameObject particleDash;

        public override void EnterState()
        {
            DisableStepOffset();
            StartCoroutine(WaitAndBackToRunWhileAirborne());
            StartCoroutine(StartDashCooldown());
        }
        IEnumerator WaitAndBackToRunWhileAirborne()
        {
            if (particleDash)
            {
                if (!particleDash.activeSelf)
                {
                    particleDash.SetActive(true);
                }
            }
            AudioInterface.PlayAudio("dash");

            yield return new WaitForSeconds(_playerMovementController.dashDuration * Time.timeScale);
           
            if (particleDash)
            {
                if (particleDash.activeSelf)
                {
                    particleDash.SetActive(false);
                }
            }
            currentSuperState.SwitchToState("RunWhileAirborne");
        }
        IEnumerator StartDashCooldown()
        {
            _playerMovementController.isDashable = false;
            yield return new WaitForSeconds(_playerMovementController.dashTimeout);
            _playerMovementController.isDashable = true;
        }

        private void DisableStepOffset()
        {
            _playerMovementController.DisableStepOffset();
        }
        private void EnableStepOffset()
        {
            _playerMovementController.EnableStepOffset();
        }

        public override void ExitState()
        {
            EnableStepOffset();
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

        protected override void InitializeComponent()
        {
            _playerMovementController = GetComponent<PlayerMovementController>();
        }

        protected override void InitializeState()
        {

        }

        protected override void PhysicsUpdateThisState()
        {

        }

        protected override void UpdateThisState()
        {
            Dash();
            CheckSwitchState();
        }

        protected override void InitializeVariable()
        {

        }
    }
}
