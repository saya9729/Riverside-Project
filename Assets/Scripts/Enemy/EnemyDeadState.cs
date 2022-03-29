using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class EnemyDeadState : AbstractClass.State
    {
        private EnemyStateManager _enemyStateManager;

        private void Start()
        {
            _enemyStateManager = GameObject.Find("EnemyStateManager").GetComponent<EnemyStateManager>();
        }
        public override void EnterState()
        {
            Debug.Log("Enter Dead State");
        }

        public override void UpdateState()
        {
            
        }

        public override void ExitState()
        {
            Debug.Log("Exit Dead State");
        }
        public override void PhysicsUpdateState()
        {

        }
    }
}