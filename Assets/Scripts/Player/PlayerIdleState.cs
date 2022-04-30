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
            _playerStateManager.playerAnimator.SetInteger("attack", 0); //return to idle
        }

        public override void UpdateState()
        {
            if(_playerStateManager.inputManager.primaryLightAttack)
            {
                _playerStateManager.SwitchState(_playerStateManager.playerPrimaryLightAttackState);
            }
            if(_playerStateManager.inputManager.secondaryAttack)
            {
                _playerStateManager.SwitchState(_playerStateManager.playerSecondaryAttackState);
            }

        }

        public override void ExitState()
        {
            
        }
        public override void PhysicsUpdateState()
        {

        }


    }
}
