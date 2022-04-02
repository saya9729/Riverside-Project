using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class EnemyAttackState : AbstractClass.State
    {
        private EnemyStateManager _enemyStateManager;

        private void Start()
        {
            _enemyStateManager = GetComponent<EnemyStateManager>();
        }
        public override void EnterState()
        {
            Debug.Log("Enter Attack State");
            //start attack animation
            _enemyStateManager.animator.SetBool("isAttacking",true);
            StartCoroutine(WaitAndBackToPatrol());
        }

        IEnumerator WaitAndBackToPatrol()
        {
            float attackLength = 0;
            foreach (AnimationClip clip in _enemyStateManager.animationClips)
            {
                if (clip.name == "Zombie Punching")
                {
                    attackLength = clip.length;
                }
            }
            yield return new WaitForSeconds(attackLength);
            _enemyStateManager.SwitchState(_enemyStateManager.enemyPatrolState);
        }

        public override void UpdateState()
        {

        }

        public override void ExitState()
        {
            Debug.Log("Exit Attack State");
            //cancel attack animation
            _enemyStateManager.animator.SetBool("isAttacking", false);
        }
        public override void PhysicsUpdateState()
        {

        }
    }
}
