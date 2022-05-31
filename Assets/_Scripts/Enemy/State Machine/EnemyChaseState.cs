using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class EnemyChaseState : AbstractClass.StateNew
    {
        private EnemyStateManager _enemyStateManager;
        [SerializeField] private float _rangeToStartAttacking = 1f;
                
        public override void EnterState()
        {
            //Debug.Log("Enemy Enter Chase State");
            _enemyStateManager.animator.SetTrigger("Chase");
            _enemyStateManager.navMeshAgent.isStopped = false;
        }

        private void UpdateChaseDestination()
        {
            if (!_enemyStateManager.navMeshAgent.pathPending)
            {
                _enemyStateManager.navMeshAgent.SetDestination(_enemyStateManager.player.transform.position);
            }
        }

        protected override void UpdateThisState()
        {
            CheckSwitchState();
            UpdateChaseDestination();
        }

        private bool IsPlayerInRangeToStartAttacking()
        {
            return Vector3.Distance(transform.position, _enemyStateManager.player.transform.position) < _rangeToStartAttacking;
        }

        public override void ExitState()
        {
            //Debug.Log("Enemy Exit Chase State");
            _enemyStateManager.navMeshAgent.isStopped = true;
        }        

        protected override void PhysicsUpdateThisState()
        {
            
        }

        protected override void CheckSwitchState()
        {
            if (IsPlayerInRangeToStartAttacking())
            {
                _enemyStateManager.SwitchToState("AttackState");
            }
            
        }

        protected override void InitializeState()
        {
            
        }

        protected override void InitializeComponent()
        {
            _enemyStateManager = GetComponent<EnemyStateManager>();
        }

        protected override void InitializeVariable()
        {
            
        }

        public override void SwitchToState(string p_StateType)
        {
            throw new NotImplementedException();
        }
    }
}