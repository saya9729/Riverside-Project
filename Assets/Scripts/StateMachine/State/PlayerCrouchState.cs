using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerCrouchState : State
    {
        private PlayerStateManager _playerStateManager;

        private Animator anim;

        private void Start()
        {
            _playerStateManager = GameObject.Find("PlayerStateManager").GetComponent<PlayerStateManager>();
            anim = GameObject.Find("PlayerCapsule").GetComponent<Animator>(); //testing crouch
        }

        public override void EnterState()
        {
            anim.SetBool("crouch", true);
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
            anim.SetBool("crouch", false);
        }
    }
}
