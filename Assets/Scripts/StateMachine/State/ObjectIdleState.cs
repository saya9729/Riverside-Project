using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class ObjectIdleState : State
    {
        private PlayerStateManager _playerStateManager;
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


        private void Start()
        {
            _playerStateManager = GameObject.Find("PlayerStateManager").GetComponent<PlayerStateManager>();
        }
    }
}
