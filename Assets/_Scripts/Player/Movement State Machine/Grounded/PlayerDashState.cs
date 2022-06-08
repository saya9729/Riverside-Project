using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerDashState : AbstractClass.StateNew
    {
        private PlayerMovementController _playerMovementController;        
        public override void EnterState()
        {
            _playerMovementController.SetDashSpeed();
            _playerMovementController.ResetAirborneDirection();
            _playerMovementController.DisableJump();
            _playerMovementController.SetDashDirection();
            _playerMovementController.DisableInput();
            _playerMovementController.ResetInputDirection();
        }        

        public override void ExitState()
        {
            _playerMovementController.EnableJump();
            _playerMovementController.EnableInput();
        }

        protected override void PhysicsUpdateThisState()
        {

        }

        protected override void CheckSwitchState()
        {
            if (!_playerMovementController.isGrounded)
            {
                currentSuperState.SwitchToState("DashWhileAirborne");                
            }
            else if (!_playerMovementController.isInDashState)
            {
                _playerMovementController.SetRunSpeed();
                currentSuperState.SwitchToState("Run");
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
            CheckSwitchState();
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