using System;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    [RequireComponent(typeof(EnemyPatrolState))]
    [RequireComponent(typeof(EnemyChaseState))]
    [RequireComponent(typeof(EnemyWaitState))]
    [RequireComponent(typeof(EnemyAttackState))]
    [RequireComponent(typeof(EnemyDeadState))]
    [RequireComponent(typeof(EnemyStatisticManager))]
    [RequireComponent(typeof(RagdollManager))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Universal.AttackManager))]
    [RequireComponent(typeof(EnemyAttackedHandler))]
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyStateManager : AbstractClass.StateNew
    {
        private EnemyPatrolState _enemyPatrolState;
        private EnemyChaseState _enemyChaseState;
        private EnemyAttackState _enemyAttackState;
        private EnemyDeadState _enemyDeadState;
        private EnemyWaitState _enemyWaitState;

        private EnemyStatisticManager _enemyStatisticManager;
        private RagdollManager _ragdollManager;
        [NonSerialized] public Universal.AttackManager enemyAttackManager;
        [NonSerialized] public Animator animator;        
        [NonSerialized] public NavMeshAgent navMeshAgent;

        [NonSerialized] public GameObject player;
        [NonSerialized] public AnimationClip[] animationClips;
        [NonSerialized] public Transform targetDestination;

        [SerializeField] private float attackRange = 1f;
        [SerializeField] private float chaseRange = 10f;
        [SerializeField] private float aggroRange = 10f;
        [SerializeField] private float faceTargetRotateSpeed = 1f;

        public float solValue = 10f;

        protected override void InitializeVariable()
        {
            animationClips = animator.runtimeAnimatorController.animationClips;
            player = GameObject.FindGameObjectWithTag("Player");
        }

        protected override void InitializeState()
        {
            _enemyPatrolState = GetComponent<EnemyPatrolState>();
            _enemyChaseState = GetComponent<EnemyChaseState>();
            _enemyAttackState = GetComponent<EnemyAttackState>();
            _enemyDeadState = GetComponent<EnemyDeadState>();
            _enemyWaitState = GetComponent<EnemyWaitState>();

            _enemyPatrolState.SetSuperState(this);
            _enemyChaseState.SetSuperState(this);
            _enemyAttackState.SetSuperState(this);
            _enemyDeadState.SetSuperState(this);
            _enemyWaitState.SetSuperState(this);

            currentSubState = _enemyPatrolState;
            currentSubState.EnterState();
        }

        protected override void InitializeComponent()
        {
            _enemyStatisticManager = GetComponent<EnemyStatisticManager>();
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            _ragdollManager = GetComponent<RagdollManager>();
            DisableRagdoll();
            enemyAttackManager = GetComponent<Universal.AttackManager>();
            DisableAttackHitbox();
        }

        public void ReceiveDamage(float p_damage)
        {
            _enemyStatisticManager.DecreaseHealth(p_damage);
            if (_enemyStatisticManager.HealthPercentage() <= 0)
            {
                SwitchToState("DeadState");
            }
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

        public override void SwitchToState(string p_stateType)
        {
            switch (p_stateType)
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
                case "WaitState":
                    SetSubState(_enemyWaitState);
                    break;                
                default:
                    break;
            }
        }

        public void EnableRagdoll()
        {
            animator.enabled = false;
            _ragdollManager.EnableRagdoll();
        }

        public void DisableRagdoll()
        {
            _ragdollManager.DisableRagdoll();
        }
        public void DisableAttackHitbox()
        {
            enemyAttackManager.DisableHitbox();
        }
        public void EnableAttackHitbox()
        {
            enemyAttackManager.EnableHitbox();
        }
        

        public bool IsPlayerInAttackRange()
        {
            return Vector3.Distance(transform.position, player.transform.position) < attackRange;
        }
        public bool IsPlayerInChaseRange()
        {
            return Vector3.Distance(transform.position, player.transform.position) < chaseRange;
        }
        public bool IsPlayerInAggroRange()
        {
            return Vector3.Distance(transform.position, player.transform.position) < aggroRange;
        }

        public void LookAtTarget()
        {
            Vector3 lookPos = targetDestination.position - transform.position;
            lookPos.y = 0;

            Quaternion rot = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * faceTargetRotateSpeed);
        }

        public void StopMoving()
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
        }

        public void MoveTo(Vector3 p_destination)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(p_destination);
        }

    }
}