using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerIdleStateOld : AbstractClass.StateOld
    {
        private PlayerStateManagerOld _playerStateManager;

        private void Start()
        {
            _playerStateManager = GameObject.Find("PlayerStateManager").GetComponent<PlayerStateManagerOld>();
        }

        public override void EnterState()
        {
            
        }

        public override void UpdateState()
        {
            if (_playerStateManager.inputManager.interact) {
                if (_playerStateManager.selectionManager.currentCenterScreenObject != null)
                {
                    if (_playerStateManager.selectionManager.currentCenterScreenObject.CompareTag(_playerStateManager.DragableTag) && _playerStateManager.selectionManager.distanceToCenterScreenObject<_playerStateManager.playerDragState.dragRange)
                    {
                        _playerStateManager.SwitchState(_playerStateManager.playerDragState);
                    }
                    else if (_playerStateManager.selectionManager.currentCenterScreenObject.CompareTag(_playerStateManager.InspectableTag) && _playerStateManager.selectionManager.distanceToCenterScreenObject < _playerStateManager.playerInspectState.inspectRange)
                    {
                        _playerStateManager.SwitchState(_playerStateManager.playerInspectState);
                    }
                }
            }
            else
            {
                //Debug.Log("Unhandled input");
            }
        }

        public override void ExitState()
        {

        }
        public override void PhysicsUpdateState()
        {

        }
    }
}
