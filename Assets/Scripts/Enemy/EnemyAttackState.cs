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
            Debug.Log("Enter Attack State");
            //start attack animation
        }

        public override void UpdateState()
        {

        }

        public override void ExitState()
        {
            Debug.Log("Exit Attack State");
        }
        public override void PhysicsUpdateState()
        {

        }
    }
}
