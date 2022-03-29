using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerPocketWatchState : AbstractClass.State
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

        }

        public override void ExitState()
        {

        }
        public override void PhysicsUpdateState()
        {

        }
    }
}
