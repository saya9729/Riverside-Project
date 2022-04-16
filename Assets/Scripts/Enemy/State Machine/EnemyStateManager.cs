using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyStateManager : AbstractClass.StateMachineManager
    {
        [NonSerialized] public EnemyPatrolState enemyPatrolState;
        [NonSerialized] public EnemyChaseState enemyChaseState;
        [NonSerialized] public EnemyAttackState enemyAttackState;
        [NonSerialized] public EnemyStaggerState enemyStaggerState;
        [NonSerialized] public EnemyDeadState enemyDeadState;

        private EnemyStatisticManager _enemyStatisticManager;
        [NonSerialized] public Animator animator;

        [NonSerialized] public NavMeshAgent navMeshAgent;
        [NonSerialized] public LayerMask playerLayerMask;
        public GameObject player;
        [NonSerialized] public AnimationClip[] animationClips;

        private void Start()
        {
            InitializeManager();

            InitializeVariable();

            InitializeState();

            


            _currentState = enemyPatrolState;
            _currentState.EnterState();
        }
        void InitializeVariable()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            playerLayerMask = LayerMask.GetMask("Player");
            //player = GameObject.Find("MainPlayer");
            animationClips = animator.runtimeAnimatorController.animationClips;
        }

        void InitializeState()
        {
            enemyStaggerState = GetComponent<EnemyStaggerState>();
            enemyPatrolState = GetComponent<EnemyPatrolState>();
            enemyChaseState = GetComponent<EnemyChaseState>();
            enemyAttackState = GetComponent<EnemyAttackState>();
            enemyDeadState = GetComponent<EnemyDeadState>();
        }

        void InitializeManager()
        {
            _enemyStatisticManager = GetComponent<EnemyStatisticManager>();
            animator = GetComponent<Animator>();
        }

        public void ReceiveDamage(float p_damage)
        {
            _enemyStatisticManager.DecreaseHealth(p_damage);
            if (_enemyStatisticManager.HealthPercentage() < 0)
            {
                SwitchState(enemyDeadState);
            }
            else if (_enemyStatisticManager.HealthPercentage() < enemyStaggerState.healthStaggerThreshold)
            {
                SwitchState(enemyStaggerState);
            }
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