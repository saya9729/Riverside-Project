using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class EnemyWaitState : AbstractClass.State
    {
        private EnemyStateManager _enemyStateManager;

        [SerializeField] private float waitAtWaypointTime =5f;
        public override void EnterState()
        {
            //Debug.Log("Enemy Enter Wait At Waypoint State");
            _enemyStateManager.animator.SetTrigger("Wait");
            StartCoroutine(WaitAndBackToPatrol());
        }

        IEnumerator WaitAndBackToPatrol()
        {
            yield return new WaitForSeconds(waitAtWaypointTime);
            _enemyStateManager.SwitchToState("PatrolState");
        }

        public override void ExitState()
        {
            //Debug.Log("Enemy Exit Wait At Waypoint State");
            StopAllCoroutines();
        }        

        public override void SwitchToState(string p_StateType)
        {
            throw new System.NotImplementedException();
        }

        protected override void CheckSwitchState()
        {
            if (_enemyStateManager.IsPlayerInAggroRange())
            {
                _enemyStateManager.SwitchToState("ChaseState");
            }
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
            CheckSwitchState();
        }
    }
}
