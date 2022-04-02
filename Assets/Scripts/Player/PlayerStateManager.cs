using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Player
{
    public class PlayerStateManager : AbstractClass.StateMachineManager
    {
        //Manager
        [NonSerialized] public SelectionManager selectionManager;
        [NonSerialized] public PlayerSkillManager playerSkillManager;
        [NonSerialized] public InputManager inputManager;
        [NonSerialized] public PlayerStatisticManager playerStatisticManager;

        [NonSerialized] public PlayerIdleState playerIdleState;
        [NonSerialized] public PlayerPrimaryLightAttackState playerPrimaryLightAttackState;
        [NonSerialized] public PlayerSecondaryAttackState playerSecondaryAttackState;
        [NonSerialized] public PlayerBlockState playerBlockState;

        private void Start()
        {
            InitializeManager();

            InitializeState();


            _currentState = playerIdleState;
            _currentState.EnterState();
        }

        void InitializeManager()
        {
            inputManager = GetComponentInParent<InputManager>();
            playerSkillManager = GameObject.Find("Manager").GetComponent<PlayerSkillManager>();
            playerStatisticManager= GetComponent<PlayerStatisticManager>();
        }

        void InitializeState()
        {
            playerIdleState = GetComponent<PlayerIdleState>();
            playerPrimaryLightAttackState = GetComponent<PlayerPrimaryLightAttackState>();
            playerSecondaryAttackState = GetComponent<PlayerSecondaryAttackState>();
            playerBlockState = GetComponent<PlayerBlockState>();
        }

        private void FixedUpdate()
        {
            _currentState.PhysicsUpdateState();
        }

        void Update()
        {
            _currentState.UpdateState();
            if (inputManager.usingPocketWatch)
            {
                playerSkillManager.SlowTime();
            }
        }
    }
}
