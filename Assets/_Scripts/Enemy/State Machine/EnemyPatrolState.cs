using UnityEngine;
namespace Enemy
{
    public class EnemyPatrolState : AbstractClass.State
    {
        private EnemyStateManager _enemyStateManager;

        [SerializeField] private Transform[] waypoints;
        [SerializeField] private float brakingDistance = 1f;

        private int _waypointIndex=0;
        private Transform _targetDestination;

        public override void EnterState()
        {
            //Debug.Log("Enemy Enter Patrol State");
            try
            {
                _enemyStateManager.animator.SetTrigger("Patrol");
            }
            catch
            {
                //In case the call for EnterState() came before Start()
                Start();
                _enemyStateManager.animator.SetTrigger("Patrol");
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

            _enemyStateManager.MoveTo(_targetDestination.position);
            _waypointIndex = waypoints.Length != 0 ? (_waypointIndex + 1) % waypoints.Length : 0;
        }

        public override void ExitState()
        {
            //Debug.Log("Enemy Exit Patrol State");
            _enemyStateManager.StopMoving();
        }
        
        protected override void PhysicsUpdateThisState()
        {
            
        }

        protected override void CheckSwitchState()
        {
            if (_enemyStateManager.IsPlayerInAggroRange())
            {
                _enemyStateManager.SwitchToState("ChaseState");
            }
            else if (Vector3.Distance(transform.position, _targetDestination.position) < brakingDistance)
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
            throw new System.NotImplementedException();
        }        
    }
}
