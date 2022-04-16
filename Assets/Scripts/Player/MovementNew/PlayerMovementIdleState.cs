using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerMovementIdleState : AbstractClass.StateNew
    {
        private PlayerMovementController _playerMovementController;
        public override void EnterState()
        {
            
        }
        public override void ExitState()
        {
            
        }
        protected override void PhysicsUpdateThisState()
        {
            
        }
        public override void SwitchToState(string p_StateType)
        {
            throw new System.NotImplementedException();
        }
        protected override void CheckSwitchState()
        {
            if (_playerMovementController.inputManager.move != Vector2.zero)
            {
                currentSuperState.SwitchToState("Run");
            }
            else if (!_playerMovementController.isGrounded)
            {
                currentSuperState.SwitchToState("IdleWhileAirborne");
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
        protected override void UpdateThisState()
        {
            CheckSwitchState();
        }
    }
}
