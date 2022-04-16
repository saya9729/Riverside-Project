using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class ObjectDragState : AbstractClass.State
    {
        private PlayerStateManagerOld _playerStateManager;
        private ObjectManipulationBody _currentObjectBody;

        public override void EnterState()
        {
            _currentObjectBody = _playerStateManager.selectionManager.currentCenterScreenObject.GetComponent<ObjectManipulationBody>();
            _currentObjectBody.StartDragObject();
        }

        public override void UpdateState()
        {
            
        }

        public override void ExitState()
        {
            _currentObjectBody.StopDragObject();
            _currentObjectBody = null;
        }        

        private void Start()
        {
            _playerStateManager = GameObject.Find("PlayerStateManager").GetComponent<PlayerStateManagerOld>();
        }

        public override void PhysicsUpdateState()
        {
            _currentObjectBody.UpdateObjectPosition();
        }
    }
}
