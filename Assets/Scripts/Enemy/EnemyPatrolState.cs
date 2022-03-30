using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class EnemyPatrolState : AbstractClass.State
    {
        private EnemyStateManager _enemyStateManager;

        [SerializeField] private Transform[] waypoints;
        private int _waypointIndex;
        private Vector3 _targetDestination;
        private float _sightRadius;

        private void Start()
        {
            _enemyStateManager = GameObject.Find("EnemyStateManager").GetComponent<EnemyStateManager>();
            InitializeVariable();
        }

        private void InitializeVariable()
        {
            
        }
        public override void EnterState()
        {
            Debug.Log("Enter Patrol State");
            UpdateDestination();
        }

        public override void UpdateState()
        {
            if (Vector3.Distance(transform.position, _targetDestination) < 1)
            {
                UpdateDestination();
            }

            if (IsPlayerVisible())
            {
                _enemyStateManager.SwitchState(_enemyStateManager.enemyChaseState);
            }
            else
            {
                //continue patroling
            }
        }

        void UpdateDestination()
        {
            _targetDestination = waypoints[_waypointIndex].position;
            _enemyStateManager.navMeshAgent.SetDestination(_targetDestination);
            _waypointIndex = waypoints.Length != 0 ? (_waypointIndex + 1) % waypoints.Length : 0;
        }

        private bool IsPlayerVisible()
        {            
            return Physics.CheckSphere(transform.position,_sightRadius,_enemyStateManager.playerLayerMask);
        }

        public override void ExitState()
        {
            Debug.Log("Exit Patrol State");
        }
        public override void PhysicsUpdateState()
        {

        }
    }
}
