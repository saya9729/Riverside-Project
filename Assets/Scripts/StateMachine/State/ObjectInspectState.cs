using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class ObjectInspectState : State
    {
        private PlayerStateManager _playerStateManager;
        private ObjectManipulationBody _currentObjectBody;
        public override void EnterState()
        {
            _currentObjectBody = _playerStateManager.selectionManager.currentCenterScreenObject.GetComponent<ObjectManipulationBody>();
            _currentObjectBody.StartInspectObject();
        }

        public override void UpdateState()
        {
            _currentObjectBody.RotateInspectingObject();
        }

        public override void ExitState()
        {
            _currentObjectBody.StopInspectObject();
            _currentObjectBody = null;
        }

        private void Start()
        {
            _playerStateManager = GameObject.Find("PlayerStateManager").GetComponent<PlayerStateManager>();
        }
    }
}
