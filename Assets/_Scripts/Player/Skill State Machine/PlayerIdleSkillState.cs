using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerIdleSkillState : AbstractClass.State
    {
        private PlayerSkillStateManager _playerSkillStateManager;
        public override void EnterState()
        {

        }

        public override void ExitState()
        {

        }

        public override void SwitchToState(string p_stateType)
        {

        }

        protected override void CheckSwitchState()
        {
            if (_playerSkillStateManager.inputManager.usingPocketWatch&&_playerSkillStateManager.IsSlowTimeAble())
            {
                currentSuperState.SwitchToState("SlowTime");
            }
        }

        protected override void InitializeComponent()
        {
            _playerSkillStateManager = GetComponent<PlayerSkillStateManager>();
        }

        protected override void InitializeState()
        {

        }

        protected override void InitializeVariable()
        {

        }

        protected override void PhysicsUpdateThisState()
        {

        }

        protected override void UpdateThisState()
        {
            CheckSwitchState();
        }
    }
}