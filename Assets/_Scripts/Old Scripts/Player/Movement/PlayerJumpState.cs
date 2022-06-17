using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerJumpState : AbstractClass.StateOld
    {
        private PlayerActionStateManager _playerStateManager;

        private void Start()
        {
            _playerStateManager = GameObject.Find("PlayerStateManager").GetComponent<PlayerActionStateManager>();
        }

        public override void EnterState()
        {
            Debug.Log("enter idle state");

        }

        public override void UpdateState()
        {

        }

        public override void ExitState()
        {
            Debug.Log("exit idle state");

        }
        public override void PhysicsUpdateState()
        {

        }
    }
}
