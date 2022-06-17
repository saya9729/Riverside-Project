using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerAttackState : AbstractClass.State
    {
        private PlayerActionStateManager _playerStateManager;

        [Header("Primary Attack (Slash)")]

        int _attackTypeIndex = 0;

        #region State Machine
        public override void EnterState()
        {
            // TODO: handle change animation between attack pattern
            _playerStateManager.animator.SetInteger("attackType", _attackTypeIndex);
            _playerStateManager.animator.SetTrigger("isAttack");
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