using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerDragState : AbstractClass.State
    {
        private PlayerStateManager _playerStateManager;

        public float distanceFromPlayerToObject = 5.0f;
        public float catchUpVelocity = 10.0f;

        private void Start()
        {
            _playerStateManager = GameObject.Find("PlayerStateManager").GetComponent<PlayerStateManager>();
        }

        public override void EnterState()
        {
            _playerStateManager.objectManipulatorStateManager.SwitchState(_playerStateManager.objectManipulatorStateManager.objectDragState);
        }

        public override void UpdateState()
        {
            if (!_playerStateManager.inputManager.interact)
            {
                _playerStateManager.SwitchState(_playerStateManager.playerIdleState);
            }
            else
            {
                // object manipulator automatic update state
            }
        }

        public override void PhysicsUpdateState()
        {

        }

        public override void ExitState()
        {
            _playerStateManager.objectManipulatorStateManager.SwitchState(_playerStateManager.objectManipulatorStateManager.objectIdleState);
        }        
    }
}
