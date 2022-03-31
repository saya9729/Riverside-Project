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
        [NonSerialized] public InputManager inputManager;
        [NonSerialized] public TimeManager timeManager;

        [NonSerialized] public PlayerIdleState playerIdleState;
        [NonSerialized] public PlayerPrimaryLightAttackState playerPrimaryLightAttackState;
        [NonSerialized] public PlayerSecondaryAttackState playerSecondaryAttackState;
        [NonSerialized] public PlayerBlockState playerBlockState;
        [NonSerialized] public PlayerPocketWatchState playerPocketWatchState;

        private void Start()
        {
            InitializeManager();

            InitializeState();


            _currentState = playerIdleState;
            _currentState.EnterState();
        }

        void InitializeManager()
        {
            selectionManager = GameObject.Find("SelectionManager").GetComponent<SelectionManager>();
            //PlayerCapsule is the name of the object contain Player Input Component and cannot be change because of the Starter assets script
            inputManager = GetComponentInParent<InputManager>();
            timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        }

        void InitializeState()
        {
            playerIdleState = GetComponent<PlayerIdleState>();
            playerPrimaryLightAttackState = GetComponent<PlayerPrimaryLightAttackState>();
            playerSecondaryAttackState = GetComponent<PlayerSecondaryAttackState>();
            playerBlockState = GetComponent<PlayerBlockState>();
            playerPocketWatchState = GetComponent<PlayerPocketWatchState>();
        }

        private void FixedUpdate()
        {
            _currentState.PhysicsUpdateState();
        }

        void Update()
        {
            _currentState.UpdateState();
        }
    }
}
