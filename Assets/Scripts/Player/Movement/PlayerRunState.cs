using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerRunState : AbstractClass.State
    {
        private PlayerStateManager _playerStateManager;
        private PMovementStateMachine _pMovementStateMachine;

        private void Start()
        {
            _playerStateManager = GameObject.Find("PlayerStateManager").GetComponent<PlayerStateManager>();
            _pMovementStateMachine = GetComponent<PMovementStateMachine>();
        }

        public override void EnterState()
        {
            Debug.Log("enter run state");

        }

        public override void UpdateState()
        {
            if (_playerStateManager.inputManager.dodge)
            {
                _pMovementStateMachine.SwitchState(_pMovementStateMachine.playerDodgeState);
            }
            else
            {
                _playerStateManager.SwitchState(_playerStateManager.playerIdleState);
            }
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
