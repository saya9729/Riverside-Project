using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerRunWhileAirborneState : AbstractClass.StateNew
    {
        private PlayerMovementController _playerMovementController;
        public override void EnterState()
        {
            
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
                currentSuperState.SwitchToState("Run");
            }
            else if (_playerMovementController._input.move==Vector2.zero)
            {
                currentSuperState.SwitchToState("IdleWhileAirborne");
            }
            else if (_playerMovementController._isDashable && _playerMovementController._input.dash)
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
