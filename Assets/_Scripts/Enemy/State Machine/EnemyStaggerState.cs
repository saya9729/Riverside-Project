using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class EnemyStaggerState : AbstractClass.StateNew
    {
        private EnemyStateManager _enemyStateManager;
        public float healthStaggerThreshold = 80;

        public override void EnterState()
        {
            //Debug.Log("Enemy Enter Stagger State");
            //start stagger animation once
            _enemyStateManager.animator.SetTrigger("Stagger");
            StartCoroutine(WaitAndBackToPatrol());
        }
        IEnumerator WaitAndBackToPatrol()
        {
            float staggerLength = 0;
            foreach(AnimationClip clip in _enemyStateManager.animationClips)
            {
                if (clip.name == "Stagger")
                {
                    staggerLength = clip.length;
                }
            }
            yield return new WaitForSeconds(staggerLength);            
            _enemyStateManager.SwitchToState("PatrolState");
        }
        
        public override void ExitState()
        {
            //Debug.Log("Enemy Exit Stagger State");
        }        

        protected override void UpdateThisState()
        {
            
        }

        protected override void PhysicsUpdateThisState()
        {
            
        }

        protected override void CheckSwitchState()
        {
            
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
