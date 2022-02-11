using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Player
{
    public class PlayerStateManager : StateMachineManager
    {
        public string DragableTag = "Dragable";
        public string InspectableTag = "Inspectable";
        private PlayerManager _playerManager;

        [NonSerialized] public PlayerIdleState playerIdleState = new PlayerIdleState();
        [NonSerialized] public PlayerDragState playerDragState = new PlayerDragState();

        private void Start()
        {
            SetPlayerManagerForAllState(_playerManager);
            _currentState = playerIdleState;
            _currentState.EnterState();
        }
        public void SetPlayerManager(PlayerManager p_playerManager)
        {
            _playerManager = p_playerManager;
        }

        void Update()
        {
            _currentState.UpdateState();
        }
        void SetPlayerManagerForAllState(PlayerManager p_playerManager)
        {
            playerIdleState.SetPlayerManager(p_playerManager);
            playerDragState.SetPlayerManager(p_playerManager);
        }
    }
}
