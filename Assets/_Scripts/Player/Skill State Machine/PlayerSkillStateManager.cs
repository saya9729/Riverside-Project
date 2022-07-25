using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerIdleSkillState))]
    [RequireComponent(typeof(PlayerSlowTimeSkillState))]

    [RequireComponent(typeof(InputManager))]
    public class PlayerSkillStateManager : AbstractClass.State
    {
        [SerializeField] private float timeScaleWhileTimeSlow = 0.5f;
        [SerializeField] private float timeSlowDuration = 2f;
        [SerializeField] private float timeSlowCooldown = 2f;
        [SerializeField][Range(0, 1)] private float timeSlowRevertThreshold = 0.5f;



        public InputManager inputManager;

        private PlayerMovementStateManager _playerMovementStateManager;

        private PlayerIdleSkillState _playerIdleSkillState;
        private PlayerSlowTimeSkillState _playerSlowTimeSkillState;

        private bool _isSlowTimeAble = true;
        #region State Machine
        public override void EnterState()
        {

        }

        protected override void UpdateThisState()
        {

        }

        protected override void PhysicsUpdateThisState()
        {

        }

        public override void ExitState()
        {

        }

        protected override void CheckSwitchState()
        {

        }

        protected override void InitializeState()
        {
            _playerIdleSkillState = GetComponent<PlayerIdleSkillState>();
            _playerSlowTimeSkillState = GetComponent<PlayerSlowTimeSkillState>();

            _playerIdleSkillState.SetSuperState(this);
            _playerSlowTimeSkillState.SetSuperState(this);

            SetSuperState(null);
            SetSubState(_playerIdleSkillState);
        }

        protected override void InitializeComponent()
        {
            inputManager = GetComponent<InputManager>();
            _playerMovementStateManager = GetComponent<PlayerMovementStateManager>();
        }

        protected override void InitializeVariable()
        {

        }

        public override void SwitchToState(string p_stateType)
        {
            switch (p_stateType)
            {
                case "Idle":
                    SetSubState(_playerIdleSkillState);
                    break;
                case "SlowTime":
                    SetSubState(_playerSlowTimeSkillState);
                    break;
                default:
                    SetSubState(null);
                    break;
            }
        }
        #endregion

        #region API

        public void SetTimeSlow()
        {
            Time.timeScale = timeScaleWhileTimeSlow;
            _isSlowTimeAble = false;
        }

        public void StartCoroutineRevertTimeScale()
        {
            StartCoroutine(RevertTimeScale());
        }

        public bool IsTimeSlowed()
        {
            return currentSubState == _playerSlowTimeSkillState;
        }

        public void EnableUnlimitedDash()
        {
            _playerMovementStateManager.EnableUnlimitedDash();
        }
        public void DisableUnlimitedDash()
        {
            _playerMovementStateManager.DisableUnlimitedDash();
        }
        public bool IsSlowTimeAble()
        {
            return _isSlowTimeAble;
        }
        public void StartCoroutineStartSlowTimeCooldown()
        {
            StartCoroutine(StartSlowTimeCooldown());
        }
        #endregion

        #region Coroutine

        private IEnumerator RevertTimeScale()
        {
            yield return new WaitForSecondsRealtime(timeSlowDuration * timeSlowRevertThreshold);
            while (Time.timeScale != 1)
            {
                Time.timeScale = Universal.Smoothing.LinearSmoothFixedTime(Time.timeScale, timeScaleWhileTimeSlow, 1, Time.unscaledDeltaTime, timeSlowDuration * (1 - timeSlowRevertThreshold));
                yield return null;
            }
            SwitchToState("Idle");
        }
        
        private IEnumerator StartSlowTimeCooldown()
        {            
            yield return new WaitForSecondsRealtime(timeSlowCooldown);
            _isSlowTimeAble = true;
        }

        #endregion

        private void Update()
        {
            UpdateAllState();
        }

        private void FixedUpdate()
        {
            PhysicsUpdateAllState();
        }
    }
}