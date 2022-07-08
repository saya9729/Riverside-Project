using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerWallRunState : AbstractClass.State
    {
        private PlayerMovementStateManager _playerMovementController;
        public override void EnterState()
        {
            _playerMovementController.DisableGravity();
        }

        public override void ExitState()
        {
            _playerMovementController.EnableRunGravity();
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
            CheckSwitchState();
            _playerMovementController.MoveWhileWallRun();
        }
    }
}