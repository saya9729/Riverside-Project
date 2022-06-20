using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerActionIdleState : AbstractClass.State
    {
        private PlayerActionStateManager _playerStateManager;

        #region State Machine
        public override void EnterState()
        {
            //Debug.Log("enter idle state");
            //Start animation
            //_playerStateManager.playerAnimator.SetInteger("attack", 0); //return to idle
        }

        public override void ExitState()
        {
            //Debug.Log("exit idle state");
        }

        protected override void UpdateThisState()
        {
            CheckSwitchState();
        }

        protected override void PhysicsUpdateThisState()
        {
            
        }

        protected override void CheckSwitchState()
        {
            if (_playerStateManager.inputManager.primaryLightAttack)
            {                
                _playerStateManager.SwitchToState("Attack");
            }
            // TODO: implement shooting later
            //else if (_playerStateManager.inputManager.secondaryAttack)
            //{
            //    if (!aimManager.IsOnCooldown())
            //    {
            //        _playerStateManager.SwitchState(_playerStateManager.playerSecondaryAttackState);
            //    }
            //    else Debug.Log("Weapon on cooldown!");
            //}
        }

        protected override void InitializeState()
        {
            
        }

        protected override void InitializeComponent()
        {
            _playerStateManager = GetComponent<PlayerActionStateManager>();
        }

        protected override void InitializeVariable()
        {
            
        }

        public override void SwitchToState(string p_stateType)
        {
            throw new System.NotImplementedException();
        }
        #endregion

    }
}
