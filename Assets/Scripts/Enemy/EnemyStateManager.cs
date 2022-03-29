using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyStateManager : AbstractClass.StateMachineManager
    {
        public float sightRadius;

        [NonSerialized] public EnemyPatrolState enemyPatrolState;
        [NonSerialized] public EnemyChaseState enemyChaseState;
        [NonSerialized] public EnemyAttackState enemyAttackState;
        [NonSerialized] public EnemyStaggerState enemyStaggerState;
        [NonSerialized] public EnemyDeadState enemyDeadState;

        private EnemyStatisticManager _enemyStatisticManager;
        [NonSerialized] public NavMeshAgent _navMeshAgent;
        public LayerMask playerLayerMask;


        private void Start()
        {
            InitializeVariable();

            InitializeState();

            InitializeManager();


            _currentState = enemyPatrolState;
            _currentState.EnterState();
        }
        void InitializeVariable()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            playerLayerMask = LayerMask.GetMask("Player");
        }

        void InitializeState()
        {
            enemyStaggerState = GameObject.Find("EnemyState").GetComponent<EnemyStaggerState>();
            enemyPatrolState = GameObject.Find("EnemyState").GetComponent<EnemyPatrolState>();
            enemyChaseState = GameObject.Find("EnemyState").GetComponent<EnemyChaseState>();
            enemyAttackState = GameObject.Find("EnemyState").GetComponent<EnemyAttackState>();
            enemyDeadState = GameObject.Find("EnemyState").GetComponent<EnemyDeadState>();
        }

        void InitializeManager()
        {
            _enemyStatisticManager = GetComponent<EnemyStatisticManager>();
        }

        void Update()
        {
            _currentState.UpdateState();
        }

        private void FixedUpdate()
        {
            _currentState.PhysicsUpdateState();
        }
    }
}