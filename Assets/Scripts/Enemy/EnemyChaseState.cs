using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class EnemyChaseState : AbstractClass.State
    {
        private EnemyStateManager _enemyStateManager;
        [SerializeField] private float _rangeToStartAttacking = 1f;

        private void Start()
        {
            _enemyStateManager = GameObject.Find("EnemyStateManager").GetComponent<EnemyStateManager>();
        }
        public override void EnterState()
        {
            Debug.Log("Enter Chase State");
        }

        public override void UpdateState()
        {
            if (IsPlayerInRangeToStartAttacking())
            {
                _enemyStateManager.SwitchState(_enemyStateManager.enemyAttackState);
            }
            else
            {
                _enemyStateManager.navMeshAgent.SetDestination(_enemyStateManager.player.transform.position);
            }
        }        

        private bool IsPlayerInRangeToStartAttacking()
        {
            return Vector3.Distance(transform.position, _enemyStateManager.player.transform.position) < _rangeToStartAttacking;
        }

        public override void ExitState()
        {
            Debug.Log("Exit Chase State");
        }
        public override void PhysicsUpdateState()
        {

        }
    }
}