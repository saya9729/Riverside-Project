using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Player
{
    public class PlayerActionStateManager : AbstractClass.State
    {
        //Manager
        [NonSerialized] public SelectionManager selectionManager;

        [NonSerialized] public PlayerSkillStateManager playerSkillManager;
        [NonSerialized] public InputManager inputManager;
        [NonSerialized] public PlayerStatisticManager playerStatisticManager;
        [NonSerialized] public Volume volume;
        [NonSerialized] public PlayerInteractManager playerInteractManager;
        [NonSerialized] public Universal.AttackManager playerAttackManager;

        [NonSerialized] public PlayerActionIdleState playerActionIdleState;
        [NonSerialized] public PlayerAttackState playerAttackState;

        [NonSerialized] public Animator animator;

        #region State Machine
        public override void EnterState()
        {
            throw new NotImplementedException();
        }

        protected override void UpdateThisState()
        {
            //if (inputManager.usingPocketWatch)
            //{
            //    playerSkillManager.gameIsSlowDown = !playerSkillManager.gameIsSlowDown;
            //    playerSkillManager.ToggleSlowGame(playerSkillManager.gameIsSlowDown);
            //    inputManager.usingPocketWatch = false;
            //}
        }

        protected override void PhysicsUpdateThisState()
        {

        }

        public override void ExitState()
        {
            throw new NotImplementedException();
        }

        protected override void CheckSwitchState()
        {
            throw new NotImplementedException();
        }

        protected override void InitializeState()
        {
            playerActionIdleState = GetComponent<PlayerActionIdleState>();
            playerAttackState = GetComponent<PlayerAttackState>();

            SetSuperState(null);
            currentSubState = playerActionIdleState;
            currentSubState.EnterState();
        }

        protected override void InitializeComponent()
        {
            inputManager = GetComponent<InputManager>();
            playerSkillManager = GetComponent<PlayerSkillStateManager>();
            playerStatisticManager = GetComponent<PlayerStatisticManager>();
            playerInteractManager = GetComponent<PlayerInteractManager>();
            playerAttackManager = GetComponent<Universal.AttackManager>();
            DisableAttackHitbox();
            animator = GetComponent<Animator>();
        }

        protected override void InitializeVariable()
        {
            volume = GameObject.Find("PlayerFollowCamera").GetComponent<Volume>();
        }

        public override void SwitchToState(string p_stateType)
        {
            switch (p_stateType)
            {
                case "Idle":
                    SetSubState(playerActionIdleState);
                    break;
                case "Attack":
                    SetSubState(playerAttackState);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region API
        public void DisableAttackHitbox()
        {
            playerAttackManager.DisableHitbox();
        }
        public void EnableAttackHitbox()
        {
            playerAttackManager.EnableHitbox();
        }
        
        #endregion

        #region Unity functions
        private void FixedUpdate()
        {
            PhysicsUpdateAllState();
        }

        private void Update()
        {
            UpdateAllState();
        }
        #endregion

    }
}