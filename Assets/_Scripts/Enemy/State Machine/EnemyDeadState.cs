using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class EnemyDeadState : AbstractClass.StateNew
    {
        private EnemyStateManager _enemyStateManager;
        [SerializeField] private float timeBeforeDisappear;

        public override void EnterState()
        {
            //Debug.Log("Enemy Enter Dead State");
            _enemyStateManager.animator.SetTrigger("Dead");
            try
            {
                _enemyStateManager.player.GetComponent<PlayerStatisticManager>().IncreaseSol(_enemyStateManager.solValue);
            }
            catch 
            { 

            }

            _enemyStateManager.EnableRagdoll();
            StartCoroutine(WaitAndDestroyThisObject());
        }

        IEnumerator WaitAndDestroyThisObject()
        {
            yield return new WaitForSeconds(timeBeforeDisappear);
            Destroy(gameObject);
        }

        public override void ExitState()
        {
            //Debug.Log("Enemy Exit Dead State");
        }
        
        protected override void UpdateThisState()
        {
            
        }

        protected override void PhysicsUpdateThisState()
        {
            
        }

        protected override void CheckSwitchState()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}