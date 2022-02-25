using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class ObjectDragState : State
    {
        private PlayerStateManager _playerStateManager;

        public Transform pickUpDestination;
        public Transform destinationParent;

        private ObjectManipulationBody _currentObjectBody;
        public override void EnterState()
        {
            _currentObjectBody = _playerStateManager.currentCenterScreenObject.GetComponent<ObjectManipulationBody>();
            _currentObjectBody.StartDragObject();
        }

        public override void UpdateState()
        {
            _currentObjectBody.UpdateObjectPosition();
        }

        public override void ExitState()
        {
            _currentObjectBody.StopDragObject();
            _currentObjectBody = null;
        }        

        private void Start()
        {
            _playerStateManager = GameObject.Find("PlayerStateManager").GetComponent<PlayerStateManager>();
        }
    }
}
