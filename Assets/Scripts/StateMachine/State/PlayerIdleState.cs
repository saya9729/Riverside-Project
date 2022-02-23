using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerIdleState : State
    {
        private PlayerStateManager _playerStateManager;

        private void Start()
        {
            _playerStateManager = GameObject.Find("PlayerStateManager").GetComponent<PlayerStateManager>();
        }

        public override void EnterState()
        {
            
        }

        public override void UpdateState()
        {
            if (_playerStateManager.inputManager.interact) {
                if (_playerStateManager.currentCenterScreenObject != null)
                {
                    if (_playerStateManager.currentCenterScreenObject.CompareTag(_playerStateManager.DragableTag))
                    {
                        _playerStateManager.SwitchState(_playerStateManager.playerDragState);
                    }
                    //use later
                    //else if (_playerManager.currentCenterScreenObject.CompareTag(_playerManager.playerStateManager.InspectableTag))
                    //{
                    //    _playerManager.playerStateManager.SwitchState(_playerManager.playerStateManager.playerInspectState);
                    //}
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
    }
}
