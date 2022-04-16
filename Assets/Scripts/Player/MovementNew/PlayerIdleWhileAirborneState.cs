using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerIdleWhileAirborneState : AbstractClass.StateNew
    {
        private PlayerMovementController _playerMovementController;
        public override void EnterState()
        {
            //start animation
        }

        public override void ExitState()
        {
            //stop animation
        }

        public override void SwitchToState(string p_StateType)
        {
            throw new System.NotImplementedException();
        }

        protected override void CheckSwitchState()
        {
            if (_playerMovementController.isGrounded)
            {
                currentSuperState.SwitchToState("Idle");
            }
            else if (_playerMovementController.inputManager.move!=Vector2.zero)
            {
                currentSuperState.SwitchToState("RunWhileAirborne");
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
            CheckSwitchState();
        }
    }
}
