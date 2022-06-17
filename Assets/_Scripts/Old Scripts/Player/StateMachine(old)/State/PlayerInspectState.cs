using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerInspectState : AbstractClass.StateOld
    {
        private PlayerStateManagerOld _playerStateManager;

        public float rotateAngle = 10.0f;
        public float delayTimeUntilDestroyObject = 0.0f;
        public float inspectRange = 5.0f;
        public float distanceFromPlayerToObject = 5.0f;

        private void Start()
        {
            _playerStateManager = GameObject.Find("PlayerStateManager").GetComponent<PlayerStateManagerOld>();
        }
        public override void EnterState()
        {            
            _playerStateManager.objectManipulatorStateManager.SwitchState(_playerStateManager.objectManipulatorStateManager.objectInspectState);
        }

        public override void UpdateState()
        {
            if (_playerStateManager.inputManager.exit)
            {
                _playerStateManager.SwitchState(_playerStateManager.playerIdleState);
            }
            else
            {
                // object manipulator automatic update state
            }
        }

        public override void ExitState()
        {
            _playerStateManager.objectManipulatorStateManager.SwitchState(_playerStateManager.objectManipulatorStateManager.objectIdleState);
        }

        public override void PhysicsUpdateState()
        {

        }
    }
}
