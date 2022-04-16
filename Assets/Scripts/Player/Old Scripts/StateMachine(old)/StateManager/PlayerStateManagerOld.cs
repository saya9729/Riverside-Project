using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Player
{
    public class PlayerStateManagerOld : AbstractClass.StateMachineManager
    {
        //Manager
        [NonSerialized] public SelectionManager selectionManager;
        [NonSerialized] public ObjectManipulationStateManager objectManipulatorStateManager;
        [NonSerialized] public InputManager inputManager;

        public string DragableTag = "Dragable";
        public string InspectableTag = "Inspectable";

        [NonSerialized] public PlayerIdleStateOld playerIdleState;
        [NonSerialized] public PlayerDragState playerDragState;
        [NonSerialized] public PlayerInspectState playerInspectState;

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
            objectManipulatorStateManager = GameObject.Find("ObjectManipulationStateManager").GetComponent<ObjectManipulationStateManager>();
            //PlayerCapsule is the name of the object contain Player Input Component and cannot be change because of the Starter assets script
            inputManager = GameObject.Find("PlayerCapsule").GetComponent<InputManager>();
        }

        void InitializeState()
        {
            playerIdleState = GameObject.Find("PlayerState").GetComponent<PlayerIdleStateOld>();
            playerDragState = GameObject.Find("PlayerState").GetComponent<PlayerDragState>();
            playerInspectState = GameObject.Find("PlayerState").GetComponent<PlayerInspectState>();
        }

        void Update()
        {
            _currentState.UpdateState();
        }
    }
}
