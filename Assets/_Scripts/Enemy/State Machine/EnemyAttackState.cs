using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class EnemyAttackState : AbstractClass.StateNew
    {
        private EnemyStateManager _enemyStateManager;
        [SerializeField] private string attackAnimationName = "Jab Cross";

        public override void EnterState()
        {
            //Debug.Log("Enemy Enter Attack State");
            //start attack animation
            _enemyStateManager.animator.SetTrigger("Attack");
            StartCoroutine(WaitAndBackToPatrol());
        }

        IEnumerator WaitAndBackToPatrol()
        {
            float attackLength = 0;
            foreach (AnimationClip clip in _enemyStateManager.animationClips)
            {
                if (clip.name == attackAnimationName)
                {
                    attackLength = clip.length;
                    break;
                }
            }
            yield return new WaitForSeconds(attackLength);
            _enemyStateManager.SwitchToState("PatrolState");
        }

        public override void ExitState()
        {
            //Debug.Log("Enemy Exit Attack State");            
        }        

        protected override void UpdateThisState()
        {
            
        }

        protected override void PhysicsUpdateThisState()
        {
            
        }

        protected override void CheckSwitchState()
        {
            throw new System.NotImplementedException();
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
