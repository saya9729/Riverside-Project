using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyStateManager : AbstractClass.StateMachineManager
    {

        public LayerMask groundLayer;
        public LayerMask playerLayer;

        public Transform playerTransform;
        [NonSerialized] public Transform enemyTransform;
        [NonSerialized] public NavMeshAgent navMeshAgent;

        //patroling
        public float walkPointRange;
        [NonSerialized] public Vector3 walkPoint;
        

        [NonSerialized] public EnemyIdleState enemyIdleState;
        [NonSerialized] public EnemyPatrolState enemyPatrolState;
        [NonSerialized] public EnemyChaseState enemyChaseState;
        [NonSerialized] public EnemyAttackState enemyAttackState;

        private void Start()
        {
            InitializeVariable();

            InitializeState();


            _currentState = enemyIdleState;
            _currentState.EnterState();
        }

        void InitializeVariable()
        {
            enemyTransform = transform.parent.transform;
            navMeshAgent = enemyTransform.GetComponent<NavMeshAgent>();
        }

        void InitializeState()
        {
            enemyIdleState = GameObject.Find("EnemyState").GetComponent<EnemyIdleState>();
            enemyPatrolState = GameObject.Find("EnemyState").GetComponent<EnemyPatrolState>();
            enemyChaseState = GameObject.Find("EnemyState").GetComponent<EnemyChaseState>();
            enemyAttackState = GameObject.Find("EnemyState").GetComponent<EnemyAttackState>();
        }

        void Update()
        {
            _currentState.UpdateState();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(enemyTransform.position, walkPointRange);
        }
    }
}