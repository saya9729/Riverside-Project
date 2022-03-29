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
            Debug.Log("enter pocket watch state state");

        }

        public override void UpdateState()
        {

        }

        public override void ExitState()
        {
            Debug.Log("exit pocket watch state");
        }
        public override void PhysicsUpdateState()
        {

        }
    }
}
