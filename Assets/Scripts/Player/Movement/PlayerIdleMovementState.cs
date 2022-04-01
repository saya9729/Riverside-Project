using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerIdleMovementState : AbstractClass.State
    {
        private PlayerStateManager _playerStateManager;
        private PMovementStateMachine _pMovementStateMachine;

        private void Start()
        {
            _playerStateManager = GetComponentInChildren<PlayerStateManager>();
            _pMovementStateMachine = GetComponent<PMovementStateMachine>();
        }

        public override void EnterState()
        {
            Debug.Log("enter run state");

        }

        public override void UpdateState()
        {

        }

        public override void ExitState()
        {
            Debug.Log("exit run state");

        }
        public override void PhysicsUpdateState()
        {
            RunHandled();
        }

        void RunHandled()
        {

        }
    }
}
