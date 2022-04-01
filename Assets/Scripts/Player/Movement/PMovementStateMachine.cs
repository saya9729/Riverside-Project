using System.Collections;
using UnityEngine;
using System;

namespace Player
{
    public class PMovementStateMachine : AbstractClass.StateMachineManager
    {
        //Manager
        [NonSerialized] public InputManager inputManager;
        [NonSerialized] public TimeManager timeManager;

        [NonSerialized] public PlayerGroundedState playerGroundedState;
        [NonSerialized] public PlayerCrouchState playerCrouchState;
        [NonSerialized] public PlayerJumpState playerJumpState;
        [NonSerialized] public PlayerIdleMovementState playerIdleMovementState;
        [NonSerialized] public PlayerRunState playerRunState;
        [NonSerialized] public PlayerDodgeState playerDodgeState;

        private void Start()
        {
            InitializeManager();

            InitializeState();


            _currentState = playerIdleMovementState;
            _currentState.EnterState();
        }

        void InitializeManager()
        {
            //PlayerCapsule is the name of the object contain Player Input Component and cannot be change because of the Starter assets script
            inputManager = GetComponentInParent<InputManager>();
            timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        }

        void InitializeState()
        {
            playerIdleMovementState = GetComponent<PlayerIdleMovementState>();
            playerRunState = GetComponent<PlayerRunState>();
            playerJumpState = GetComponent<PlayerJumpState>();
            playerCrouchState = GetComponent<PlayerCrouchState>();
            playerDodgeState = GetComponent<PlayerDodgeState>();
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