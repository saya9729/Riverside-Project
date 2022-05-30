using System;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyStateManager : AbstractClass.StateNew
    {
        private EnemyPatrolState _enemyPatrolState;
        private EnemyChaseState _enemyChaseState;
        private EnemyAttackState _enemyAttackState;
        //private EnemyStaggerState _enemyStaggerState;
        private EnemyDeadState _enemyDeadState;
        private EnemyWaitAtWaypointState _enemyWaitAtWaypointState;

        private EnemyStatisticManager _enemyStatisticManager;
        private RagdollManager _ragdollManager;
        [NonSerialized] public Animator animator;

        [NonSerialized] public NavMeshAgent navMeshAgent;
        [NonSerialized] public LayerMask playerLayerMask;
        [NonSerialized] public GameObject player;
        [NonSerialized] public AnimationClip[] animationClips;
                
        protected override void InitializeVariable()
        {            
            playerLayerMask = LayerMask.GetMask("Player");
            animationClips = animator.runtimeAnimatorController.animationClips;
            player = GameObject.FindGameObjectWithTag("Player");
        }

        protected override void InitializeState()
        {
            //_enemyStaggerState = GetComponent<EnemyStaggerState>();
            _enemyPatrolState = GetComponent<EnemyPatrolState>();
            _enemyChaseState = GetComponent<EnemyChaseState>();
            _enemyAttackState = GetComponent<EnemyAttackState>();
            _enemyDeadState = GetComponent<EnemyDeadState>();
            _enemyWaitAtWaypointState = GetComponent<EnemyWaitAtWaypointState>();

            //_enemyStaggerState.SetSuperState(this);
            _enemyPatrolState.SetSuperState(this);
            _enemyChaseState.SetSuperState(this);
            _enemyAttackState.SetSuperState(this);
            _enemyDeadState.SetSuperState(this);
            _enemyWaitAtWaypointState.SetSuperState(this);

            currentSuperState = null;
            currentSubState = _enemyPatrolState;
            currentSubState.EnterState();
        }

        protected override void InitializeComponent()
        {
            _enemyStatisticManager = GetComponent<EnemyStatisticManager>();
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            _ragdollManager = GetComponent<RagdollManager>();
        }

        public void ReceiveDamage(float p_damage)
        {
            _enemyStatisticManager.DecreaseHealth(p_damage);
            if (_enemyStatisticManager.HealthPercentage() < 0)
            {
                SwitchToState("DeadState");
            }
            //else if (_enemyStatisticManager.HealthPercentage() < _enemyStaggerState.healthStaggerThreshold)
            //{
            //    SwitchToState("StaggerState");
            //}
        }

        private void Update()
        {
            UpdateAllState();
        }

        private void FixedUpdate()
        {
            PhysicsUpdateAllState();
        }

        public override void EnterState()
        {
            throw new NotImplementedException();
        }

        protected override void UpdateThisState()
        {
            
        }

        protected override void PhysicsUpdateThisState()
        {
            
        }

        public override void ExitState()
        {
            throw new NotImplementedException();
        }

        protected override void CheckSwitchState()
        {
            throw new NotImplementedException();
        }

        public override void SwitchToState(string p_StateType)
        {
            switch (p_StateType)
            {
                case "DeadState":
                    SetSubState(_enemyDeadState);
                    break;
                case "PatrolState":
                    SetSubState(_enemyPatrolState);
                    break;
                case "ChaseState":
                    SetSubState(_enemyChaseState);
                    break;
                case "AttackState":
                    SetSubState(_enemyAttackState);
                    break;
                //case "StaggerState":
                //    SetSubState(_enemyStaggerState);
                //    break;
                case "WaitAtWaypointState":
                    SetSubState(_enemyWaitAtWaypointState);
                    break;
                default:
                    break;
            }
        }

        public void EnableRagdoll()
        {
            _ragdollManager.EnableRagdoll();
        }
        public void DisableRagdoll()
        {
            _ragdollManager.DisableRagdoll();
        }
    }
}