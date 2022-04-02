using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class EnemyDeadState : AbstractClass.State
    {
        private EnemyStateManager _enemyStateManager;
        [SerializeField] private float timeBeforeDisappear;

        private void Start()
        {
            _enemyStateManager = GetComponent<EnemyStateManager>();
        }
        public override void EnterState()
        {
            Debug.Log("Enter Dead State");
            //start dead animation once
            StartCoroutine(WaitAndDestroyThisObject());
        }

        IEnumerator WaitAndDestroyThisObject()
        {
            yield return new WaitForSeconds(timeBeforeDisappear);
            Destroy(gameObject);
        }

        public override void UpdateState()
        {
            
        }

        public override void ExitState()
        {
            Debug.Log("Exit Dead State");
        }
        public override void PhysicsUpdateState()
        {

        }
    }
}