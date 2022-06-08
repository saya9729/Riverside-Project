using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerIdleState : AbstractClass.State
    {
        private PlayerStateManager _playerStateManager;
        public AimManager aimManager;

        private void Start()
        {
            _playerStateManager = GetComponent<PlayerStateManager>();
        }

        public override void EnterState()
        {
            //Debug.Log("enter idle state");
            //_playerStateManager.playerAnimator.SetInteger("attack", 0); //return to idle
        }

        public override void UpdateState()
        {

            if (_playerStateManager.inputManager.IsGetButtonDown("Primary Light Attack"))
            {
                //Debug.Log(_playerStateManager.inputManager.IsGetButtonDown("Primary Light Attack"));
                _playerStateManager.SwitchState(_playerStateManager.playerPrimaryLightAttackState);
            }
            if (_playerStateManager.inputManager.secondaryAttack)
            {
                if (!aimManager.IsOnCooldown())
                {
                    _playerStateManager.SwitchState(_playerStateManager.playerSecondaryAttackState);
                }
                else Debug.Log("Weapon on cooldown!");
            }

        }

        public override void ExitState()
        {
            //Debug.Log("exit idle state");
        }
        public override void PhysicsUpdateState()
        {

        }


    }
}
