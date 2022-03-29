using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class EnemyChaseState : AbstractClass.State
    {
        private EnemyStateManager _enemyStateManager;

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
                UpdatePlayerPosition();
                CalculateRoute();
                Chase();
            }
        }

        private void Chase()
        {
            throw new NotImplementedException();
        }

        private void CalculateRoute()
        {
            throw new NotImplementedException();
        }

        private void UpdatePlayerPosition()
        {
            throw new NotImplementedException();
        }

        private bool IsPlayerInRangeToStartAttacking()
        {
            throw new NotImplementedException();
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