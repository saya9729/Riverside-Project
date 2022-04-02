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
            _enemyStateManager.animator.SetBool("isStagger", true);
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
            _enemyStateManager.SwitchState(_enemyStateManager.enemyPatrolState);
        }

        public override void UpdateState()
        {

        }

        public override void ExitState()
        {
            Debug.Log("Exit Stagger State");
            _enemyStateManager.animator.SetBool("isStagger", false);
        }
        public override void PhysicsUpdateState()
        {

        }
    }
}
