using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class EnemyPatrolState : AbstractClass.State
    {
        private EnemyStateManager _enemyStateManager;

        private void Start()
        {
            _enemyStateManager = GameObject.Find("EnemyStateManager").GetComponent<EnemyStateManager>();
        }
        public override void EnterState()
        {
            Debug.Log("Enter Patrol State");
        }

        public override void UpdateState()
        {
            if (IsPlayerVisible())
            {
                _enemyStateManager.SwitchState(_enemyStateManager.enemyChaseState);
            }
            else
            {
                //continue patroling
            }
        }

        private bool IsPlayerVisible()
        {
            return false;
        }

        public override void ExitState()
        {
            Debug.Log("Exit Patrol State");
        }
        public override void PhysicsUpdateState()
        {

        }
    }
}
