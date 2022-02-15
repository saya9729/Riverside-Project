using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerDragState : State
    {
        private PlayerManager _playerManager;
        public override void EnterState()
        {
            _playerManager.objectManipulatorManager.PickUpObject(_playerManager.currentCenterScreenObject);
        }

        public override void UpdateState()
        {
            if (!_playerManager.inputManager.interact)
            {
                _playerManager.playerStateManager.SwitchState(_playerManager.playerStateManager.playerIdleState);
            }
            else
            {
                _playerManager.objectManipulatorManager.UpdateObjectPosition();
            }
        }

        public override void ExitState()
        {
            _playerManager.objectManipulatorManager.DropObject();
        }
        public void SetPlayerManager(PlayerManager p_playerManager)
        {
            _playerManager = p_playerManager;
        }        
    }
}
