using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class ObjectManipulationStateManager : AbstractClass.StateMachineManager
    {
        [NonSerialized] public ObjectIdleState objectIdleState;
        [NonSerialized] public ObjectDragState objectDragState;
        [NonSerialized] public ObjectInspectState objectInspectState;

        private PlayerStateManagerOld _playerStateManager;

        private void Start()
        {
            _playerStateManager = GameObject.Find("PlayerStateManager").GetComponent<PlayerStateManagerOld>();

            InitializeState();

            _currentState = objectIdleState;
            _currentState.EnterState();
        }
        
        void InitializeState()
        {
            objectIdleState = GameObject.Find("ObjectManipulationState").GetComponent<ObjectIdleState>();
            objectDragState = GameObject.Find("ObjectManipulationState").GetComponent<ObjectDragState>();
            objectInspectState = GameObject.Find("ObjectManipulationState").GetComponent<ObjectInspectState>();
        }

        private void Update()
        {
            _currentState.UpdateState();
        }

        private void FixedUpdate()
        {
            _currentState.PhysicsUpdateState();
        }
    }
}