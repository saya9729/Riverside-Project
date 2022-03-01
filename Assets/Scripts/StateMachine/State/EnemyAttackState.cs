using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyAttackState : AbstractClass.State
    {
        private EnemyStateManager _enemyStateManager;

        private void Start()
        {
            _enemyStateManager = GameObject.Find("EnemyStateManager").GetComponent<EnemyStateManager>();
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
