using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerIdleState : State
    {
        private PlayerManager _playerManager;
        public override void EnterState()
        {
            
        }

        public override void UpdateState()
        {
            if (_playerManager.inputManager.interact) {
                if (_playerManager.currentCenterScreenObject.CompareTag(_playerManager.playerStateManager.DragableTag))
                {
                    _playerManager.playerStateManager.SwitchState(_playerManager.playerStateManager.playerDragState);
                }
                //else if (_playerManager.currentCenterScreenObject.CompareTag(_playerManager.playerStateManager.InspectableTag))
                //{
                //    _playerManager.playerStateManager.SwitchState(_playerManager.playerStateManager.playerInspectState);
                //}
            }
            else
            {
                Debug.Log("Unhandled input");
            }
        }

        public override void ExitState()
        {

        }
        public void SetPlayerManager(PlayerManager p_playerManager)
        {
            _playerManager = p_playerManager;
        }
    }
}
