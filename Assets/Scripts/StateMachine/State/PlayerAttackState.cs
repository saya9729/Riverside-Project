using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerAttackState : State
    {
        private PlayerStateManager _playerStateManager;
        public bool isAttacking = false;
        public float attackValue;

        private void Start()
        {
            _playerStateManager = GameObject.Find("PlayerStateManager").GetComponent<PlayerStateManager>();
        }

        public override void EnterState()
        {
            Debug.Log("attack");
            isAttacking = true;
            //attack hit if detect trigger from enemies with AttackReceiveBody attached
        }

        public override void UpdateState()
        {
            // if (!_playerStateManager.inputManager.interact)
            // {
            //     _playerStateManager.SwitchState(_playerStateManager.playerIdleState);
            // }
            // else
            // {
            //     // object manipulator automatic update state
            // }
        }

        public override void ExitState()
        {
            isAttacking = false;
        }
    }
}
