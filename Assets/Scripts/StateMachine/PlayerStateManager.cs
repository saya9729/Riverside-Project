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
            _playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
            SetPlayerManagerForAllState(_playerManager);
            _currentState = playerIdleState;
            _currentState.EnterState();
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
