using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerAttackState : State
    {
        private PlayerStateManager _playerStateManager;

        private void Start()
        {
            _playerStateManager = GameObject.Find("PlayerStateManager").GetComponent<PlayerStateManager>();
        }

        public override void EnterState()
        {
            //enemy switch to attacked state
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

        public override void ExitState()
        {
            //enemy resumes pre-attacked state
        }        
    }
}
