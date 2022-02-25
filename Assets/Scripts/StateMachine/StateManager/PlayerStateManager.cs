using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Player
{
    public class PlayerStateManager : StateMachineManager
    {
        //Manager
        [NonSerialized] public SelectionManager selectionManager;
        [NonSerialized] public ObjectManipulationStateManager objectManipulatorStateManager;
        [NonSerialized] public InputManager inputManager;

        public string DragableTag = "Dragable";
        public string InspectableTag = "Inspectable";

        [NonSerialized] public PlayerIdleState playerIdleState;
        [NonSerialized] public PlayerDragState playerDragState;
        [NonSerialized] public PlayerItemState playerItemState;
        [NonSerialized] public PlayerAttackState playerAttackState;

        // select object
        [NonSerialized] public GameObject currentCenterScreenObject = null;

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
            playerIdleState = GameObject.Find("PlayerState").GetComponent<PlayerIdleState>();
            playerDragState = GameObject.Find("PlayerState").GetComponent<PlayerDragState>();
            playerItemState = GameObject.Find("PlayerState").GetComponent<PlayerItemState>();
            playerAttackState = GameObject.Find("PlayerState").GetComponent<PlayerAttackState>();
        }

        void Update()
        {
            currentCenterScreenObject = selectionManager.GetObjectAtScreenCenter();

            _currentState.UpdateState();
        }
    }
}
