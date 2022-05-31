using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class EnemyPatrolState : AbstractClass.StateNew
    {
        private EnemyStateManager _enemyStateManager;

        [SerializeField] private Transform[] waypoints;
        [SerializeField] private float lookAtNextDestinationRotateSpeed = 1f;
        private int _waypointIndex=0;
        private Transform _targetDestination;
        public float sightRadius = 10f;

        public override void EnterState()
        {
            //Debug.Log("Enemy Enter Patrol State");
            try
            {
                _enemyStateManager.animator.SetTrigger("Patrol");
                _enemyStateManager.navMeshAgent.isStopped = false;
                _enemyStateManager.navMeshAgent.autoBraking = false;
            }
            catch
            {
                //In case the call for EnterState() came before Start()
                Start();
                _enemyStateManager.animator.SetTrigger("Patrol");
                _enemyStateManager.navMeshAgent.isStopped = false;
                _enemyStateManager.navMeshAgent.autoBraking = false;
            }
            UpdateDestination();
        }

        protected override void UpdateThisState()
        {
            CheckSwitchState();
        }

        void UpdateDestination()
        {
            //enable this line when insert waypoints
            _targetDestination = waypoints[_waypointIndex];
            //default the object will seek the origin
            //_targetDestination.position = Vector3.zero;
            _enemyStateManager.navMeshAgent.SetDestination(_targetDestination.position);

            _waypointIndex = waypoints.Length != 0 ? (_waypointIndex + 1) % waypoints.Length : 0;
        }

        private bool IsPlayerVisible()
        {            
            return Physics.CheckSphere(transform.position,sightRadius,_enemyStateManager.playerLayerMask);
        }

        public override void ExitState()
        {
            //Debug.Log("Enemy Exit Patrol State");
            // TODO: Find a way to get rid of excessive navMeshAgent manipulation
            _enemyStateManager.navMeshAgent.isStopped = true;
        }
        
        protected override void PhysicsUpdateThisState()
        {
            
        }

        protected override void CheckSwitchState()
        {
            if (IsPlayerVisible())
            {
                _enemyStateManager.SwitchToState("ChaseState");
            }
            else
            {
                //continue patroling
            }
            
            if (Vector3.Distance(transform.position, _targetDestination.position) < 1)
            {
                _enemyStateManager.SwitchToState("WaitAtWaypointState");
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
            throw new System.NotImplementedException();
        }        
    }
}
