using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class EnemyWaitAtWaypointState : AbstractClass.StateNew
    {
        private EnemyStateManager _enemyStateManager;
        [SerializeField] private float waitAtWaypointTime =5f;
        public override void EnterState()
        {
            Debug.Log("Enemy Enter Wait At Waypoint State");
            _enemyStateManager.navMeshAgent.isStopped = true;
            StartCoroutine(WaitAndBackToPatrol());
        }

        IEnumerator WaitAndBackToPatrol()
        {
            yield return new WaitForSeconds(waitAtWaypointTime);
            _enemyStateManager.SwitchToState("PatrolState");
        }

        public override void ExitState()
        {
            Debug.Log("Enemy Exit Wait At Waypoint State");
            _enemyStateManager.navMeshAgent.isStopped = false;
        }        

        public override void SwitchToState(string p_StateType)
        {
            throw new System.NotImplementedException();
        }

        protected override void CheckSwitchState()
        {
            
        }

        protected override void InitializeComponent()
        {
            _enemyStateManager = GetComponent<EnemyStateManager>();
        }

        protected override void InitializeState()
        {
            
        }

        protected override void InitializeVariable()
        {
            
        }

        protected override void PhysicsUpdateThisState()
        {
            
        }

        protected override void UpdateThisState()
        {
            
        }
    }
}
