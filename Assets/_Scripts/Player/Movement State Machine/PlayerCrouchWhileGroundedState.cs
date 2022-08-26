using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerCrouchWhileGroundedState : AbstractClass.State
    {
        private PlayerMovementStateManager _playerMovementController;
        public override void EnterState()
        {
            _playerMovementController.SetCrouchTargetSpeed();
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
            if (!_playerMovementController.isGrounded)
            {
                _playerMovementController.EnableDoubleJump();
                currentSuperState.SwitchToState("Airborne");
            }
        }

        protected override void InitializeComponent()
        {
            _playerMovementController = GetComponent<PlayerMovementStateManager>();
        }

        protected override void InitializeState()
        {

        }

        protected override void InitializeVariable()
        {

        }

        protected override void PhysicsUpdateThisState()
        {
            _playerMovementController.MoveWhileGrounded();
        }

        protected override void UpdateThisState()
        {            
            CheckSwitchState();
        }
    }
}