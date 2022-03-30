using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class EnemyAttackState : AbstractClass.State
    {
        private EnemyStateManager _enemyStateManager;
        [SerializeField] private float attackRange;

        private void Start()
        {
            _enemyStateManager = GameObject.Find("EnemyStateManager").GetComponent<EnemyStateManager>();
        }
        public override void EnterState()
        {
            Debug.Log("Enter Attack State");            
        }

        public override void UpdateState()
        {
            if (Vector3.Distance(transform.position, _enemyStateManager.player.transform.position) < attackRange)
            {
                //start attack animation
            }
            else
            {
                _enemyStateManager.SwitchState(_enemyStateManager.enemyPatrolState);
            }
        }

        public override void ExitState()
        {
            Debug.Log("Exit Attack State");
            //cancel attack animation
        }
        public override void PhysicsUpdateState()
        {

        }
    }
}
