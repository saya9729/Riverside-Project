using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyIdleStateOld : AbstractClass.State
    {
        private EnemyStateManagerOld _enemyStateManager;

        private void Start()
        {
            _enemyStateManager = GetComponent<EnemyStateManagerOld>();
        }

        public override void EnterState()
        {
            SearchWalkPoint();
        }

        public override void UpdateState()
        {
            if (Physics.Raycast(_enemyStateManager.walkPoint, -_enemyStateManager.enemyTransform.up, 2f, _enemyStateManager.groundLayer))
            {
                _enemyStateManager.SwitchState(_enemyStateManager.enemyPatrolState);
            }
        }

        public override void ExitState()
        {

        }
        public override void PhysicsUpdateState()
        {

        }

        private void SearchWalkPoint()
        {
            float randomZ = Random.Range(-_enemyStateManager.walkPointRange, _enemyStateManager.walkPointRange);
            float randomX = Random.Range(-_enemyStateManager.walkPointRange, _enemyStateManager.walkPointRange);

            _enemyStateManager.walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        }
    }
}