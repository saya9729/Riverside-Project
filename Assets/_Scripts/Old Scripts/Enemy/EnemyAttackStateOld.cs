using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyAttackStateOld : AbstractClass.StateOld
    {
        private EnemyStateManagerOld _enemyStateManager;

        private void Start()
        {
            _enemyStateManager = GetComponent<EnemyStateManagerOld>();
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
