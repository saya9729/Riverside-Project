using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerIdleState : AbstractClass.State
    {
        private PlayerStateManager _playerStateManager;

        private void Start()
        {
            _playerStateManager = GetComponent<PlayerStateManager>();
        }

        public override void EnterState()
        {
            Debug.Log("enter idle state");
            _playerStateManager.playerAnimator.SetInteger("attack", 0); //return to idle
        }

        public override void UpdateState()
        {
            
            if(_playerStateManager.inputManager.IsGetButtonDown("Primary Light Attack"))
            {
                //Debug.Log(_playerStateManager.inputManager.IsGetButtonDown("Primary Light Attack"));
                _playerStateManager.SwitchState(_playerStateManager.playerPrimaryLightAttackState);
            }
            if(_playerStateManager.inputManager.secondaryAttack)
            {
                _playerStateManager.SwitchState(_playerStateManager.playerSecondaryAttackState);
            }

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
