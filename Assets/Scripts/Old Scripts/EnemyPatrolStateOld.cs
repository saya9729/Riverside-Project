using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyPatrolStateOld : AbstractClass.State
    {
        private EnemyStateManagerOld _enemyStateManager;

        private void Start()
        {
            _enemyStateManager = GameObject.Find("EnemyStateManager").GetComponent<EnemyStateManagerOld>();
        }

        public override void EnterState()
        {
            Patroling();
        }

        public override void UpdateState()
        {
            Vector3 distanceToWalkPoint = _enemyStateManager.enemyTransform.position - _enemyStateManager.walkPoint;
            if (distanceToWalkPoint.magnitude < 2f)
            {
                _enemyStateManager.SwitchState(_enemyStateManager.enemyIdleState);
            }
        }

        public override void PhysicsUpdateState()
        {

        }

        public override void ExitState()
        {
        }
        private void Patroling()
        {
            _enemyStateManager.navMeshAgent.SetDestination(_enemyStateManager.walkPoint);
        }


    }
}