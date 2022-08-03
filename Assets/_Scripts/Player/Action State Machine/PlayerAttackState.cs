using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerAttackState : AbstractClass.State
    {
        private PlayerActionStateManager _playerActionStateManager;
        #region State Machine
        public override void EnterState()
        {
            // TODO: handle change animation between attack pattern
            //_playerActionStateManager.animator.SetInteger("attackType", _attackTypeIndex);
            _playerActionStateManager.animator.SetTrigger("isAttack");
            _playerActionStateManager.RandomAttackAnimation();
        }

        public override void ExitState()
        {
            
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
            
        }

        protected override void InitializeState()
        {
            
        }

        protected override void InitializeComponent()
        {
            _playerActionStateManager = GetComponent<PlayerActionStateManager>();
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