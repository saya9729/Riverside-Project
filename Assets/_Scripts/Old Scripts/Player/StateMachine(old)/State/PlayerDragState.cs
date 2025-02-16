using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerDragState : AbstractClass.StateOld
    {
        private PlayerStateManagerOld _playerStateManager;

        public float dragRange = 5.0f;
        public float catchUpVelocity = 10.0f;

        [NonSerialized] public float distanceFromPlayerToObject = 5.0f;        

        private void Start()
        {
            _playerStateManager = GameObject.Find("PlayerStateManager").GetComponent<PlayerStateManagerOld>();
        }

        public override void EnterState()
        {
            _playerStateManager.objectManipulatorStateManager.SwitchState(_playerStateManager.objectManipulatorStateManager.objectDragState);
            distanceFromPlayerToObject = _playerStateManager.selectionManager.distanceToCenterScreenObject;
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
            distanceFromPlayerToObject = 5.0f;
            _playerStateManager.objectManipulatorStateManager.SwitchState(_playerStateManager.objectManipulatorStateManager.objectIdleState);
        }        
    }
}
