using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class ObjectItemState : State
    {
        private PlayerStateManager _playerStateManager;

        private ObjectManipulationBody _currentObjectBody;
        public override void EnterState()
        {
            _currentObjectBody = _playerStateManager.currentCenterScreenObject.GetComponent<ObjectManipulationBody>();
            //do something to item based on itemUseType
        }

        public override void UpdateState()
        {
        }

        public override void ExitState()
        {
            _currentObjectBody = null;
        }        

        private void Start()
        {
            _playerStateManager = GameObject.Find("PlayerStateManager").GetComponent<PlayerStateManager>();
        }
    }
}
