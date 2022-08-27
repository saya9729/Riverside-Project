using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Player
{
    [RequireComponent(typeof(PlayerActionIdleState))]
    [RequireComponent(typeof(PlayerAttackState))]
    
    public class PlayerActionStateManager : AbstractClass.State
    {
        [SerializeField] private int attackAnimationCount = 1;
        [SerializeField] private float deadEyeVariance = 0.8f;

        //Manager
        [NonSerialized] public PlayerSkillStateManager playerSkillManager;
        [NonSerialized] public InputManager inputManager;
        [NonSerialized] public PlayerStatisticManager playerStatisticManager;
        [NonSerialized] public Volume volume;
        [NonSerialized] public PlayerInteractManager playerInteractManager;
        [NonSerialized] public Universal.AttackManager playerAttackManager;

        [NonSerialized] public PlayerActionIdleState playerActionIdleState;
        [NonSerialized] public PlayerAttackState playerAttackState;

        [NonSerialized] public Animator animator;

        private int _lastAnimationIndex = 0;

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
            animator = GetComponentInChildren<Animator>();
        }

        protected override void InitializeVariable()
        {
            volume = GameObject.Find("PlayerFollowCamera").GetComponent<Volume>();
            UnityEngine.Random.InitState(DateTime.Now.Millisecond);
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

        public void RandomAttackAnimation()
        {
            int nextAnimationIndex = UnityEngine.Random.Range(0, attackAnimationCount);
            while (nextAnimationIndex == _lastAnimationIndex)
            {
                nextAnimationIndex = UnityEngine.Random.Range(0, attackAnimationCount);
            }
            _lastAnimationIndex = nextAnimationIndex;
            animator.SetInteger("attackType", nextAnimationIndex);
        }  
        
        public void DisableMovementLayer()
        {
            animator.SetLayerWeight(1, 0);
        }
        public void EnableMovementLayer()
        {
            animator.SetLayerWeight(1, 1);
        }

        public void DeadEyeEffect()
        {
            volume.weight = Mathf.PingPong(Time.time, 1 - deadEyeVariance) + deadEyeVariance;
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