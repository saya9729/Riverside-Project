using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerLedgeGrabState : AbstractClass.State
    {
        private PlayerMovementStateManager _playerMovementController;
        public override void EnterState()
        {
            _playerMovementController.DisableStepOffset();
            _playerMovementController.DisableGravity();
            _playerMovementController.SetLedgeGrabDirection();
            _playerMovementController.StartCoroutineLedgeGrabState();
        }

        public override void ExitState()
        {
            _playerMovementController.EnableStepOffset();
            _playerMovementController.EnableRunGravity();
            _playerMovementController.StopCoroutineLedgeGrabState();
        }

        public override void SwitchToState(string p_stateType)
        {
            
        }

        protected override void CheckSwitchState()
        {
            
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
            
        }

        protected override void UpdateThisState()
        {
            _playerMovementController.MoveWhileLedgeGrab();
            CheckSwitchState();
        }
    }
}
