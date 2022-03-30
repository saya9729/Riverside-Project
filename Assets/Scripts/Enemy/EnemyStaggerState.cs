using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class EnemyStaggerState : AbstractClass.State
    {
        private EnemyStateManager _enemyStateManager;

        private void Start()
        {
            _enemyStateManager = GetComponent<EnemyStateManager>();
        }
        public override void EnterState()
        {
            Debug.Log("Enter Stagger State");
            //start corotine to get back to patrol state
        }

        public override void UpdateState()
        {

        }

        public override void ExitState()
        {
            Debug.Log("Exit Stagger State");
        }
        public override void PhysicsUpdateState()
        {

        }
    }
}
