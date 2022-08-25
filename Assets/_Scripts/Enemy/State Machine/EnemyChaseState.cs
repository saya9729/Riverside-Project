using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class EnemyChaseState : AbstractClass.State
    {
        private EnemyStateManager _enemyStateManager;
    
        public override void EnterState()
        {
            //Debug.Log("Enemy Enter Chase State");
            _enemyStateManager.animator.SetTrigger("Chase");
            _enemyStateManager.SetChaseSpeed();
        }

        private void UpdateChaseDestination()
        {
            if (!_enemyStateManager.navMeshAgent.pathPending)
            {
                _enemyStateManager.MoveTo(_enemyStateManager.player.transform.position);
            }
        }

        protected override void UpdateThisState()
        {
            CheckSwitchState();
            UpdateChaseDestination();
        }       

        public override void ExitState()
        {
            //Debug.Log("Enemy Exit Chase State");
            _enemyStateManager.StopMoving();
        }        

        protected override void PhysicsUpdateThisState()
        {
            
        }

        protected override void CheckSwitchState()
        {
            if (_enemyStateManager.IsPlayerInAttackRange())
            {
                _enemyStateManager.SwitchToState("AttackState");
            }
            else if (!_enemyStateManager.IsPlayerInChaseRange())
            {
                _enemyStateManager.SwitchToState("WaitState");
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