using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyStateManagerOld : AbstractClass.StateMachineManager
    {

        public LayerMask groundLayer;
        public LayerMask playerLayer;

        public Transform playerTransform;
        [NonSerialized] public Transform enemyTransform;
        [NonSerialized] public NavMeshAgent navMeshAgent;

        //patroling
        public float walkPointRange;
        [NonSerialized] public Vector3 walkPoint;
        

        [NonSerialized] public EnemyIdleStateOld enemyIdleState;
        [NonSerialized] public EnemyPatrolStateOld enemyPatrolState;
        [NonSerialized] public EnemyChaseStateOld enemyChaseState;
        [NonSerialized] public EnemyAttackStateOld enemyAttackState;

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
            enemyIdleState = GameObject.Find("EnemyState").GetComponent<EnemyIdleStateOld>();
            enemyPatrolState = GameObject.Find("EnemyState").GetComponent<EnemyPatrolStateOld>();
            enemyChaseState = GameObject.Find("EnemyState").GetComponent<EnemyChaseStateOld>();
            enemyAttackState = GameObject.Find("EnemyState").GetComponent<EnemyAttackStateOld>();
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