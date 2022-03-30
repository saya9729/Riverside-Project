using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class EnemyStaggerState : AbstractClass.State
    {
        private EnemyStateManager _enemyStateManager;
        public float healthStaggerThreshold = 80;

        private void Start()
        {
            _enemyStateManager = GameObject.Find("EnemyStateManager").GetComponent<EnemyStateManager>();
        }
        public override void EnterState()
        {
            Debug.Log("Enter Stagger State");
            //start stagger animation once
            StartCoroutine(WaitAndBackToPatrol());
        }
        IEnumerator WaitAndBackToPatrol()
        {
            yield return new WaitForSeconds(_enemyStateManager.animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            _enemyStateManager.SwitchState(_enemyStateManager.enemyPatrolState);
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
