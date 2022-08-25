using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class EnemyAttackState : AbstractClass.State
    {
        private EnemyStateManager _enemyStateManager;

        [SerializeField] private string attackAnimationName = "Jab Cross";

        public override void EnterState()
        {
            //Debug.Log("Enemy Enter Attack State");
            _enemyStateManager.animator.SetTrigger("Attack");
            // TODO: Turn into trigger by animation event later
            //StartCoroutine(WaitAndDecide());
        }

        //IEnumerator WaitAndDecide()
        //{
        //    float attackLength = 0;
        //    foreach (AnimationClip clip in _enemyStateManager.animationClips)
        //    {
        //        if (clip.name == attackAnimationName)
        //        {
        //            attackLength = clip.length;
        //            break;
        //        }
        //    }
        //    yield return new WaitForSeconds(attackLength);
        //    CheckSwitchState();
        //}

        public override void ExitState()
        {
            //Debug.Log("Enemy Exit Attack State");            
        }        

        protected override void UpdateThisState()
        {
            _enemyStateManager.targetDestination = _enemyStateManager.player.transform;
            _enemyStateManager.LookAtTarget();
        }

        protected override void PhysicsUpdateThisState()
        {
            
        }

        protected override void CheckSwitchState()
        {
            //if (_enemyStateManager.IsPlayerInAttackRange())
            //{
            //    _enemyStateManager.SwitchToState("AttackState");
            //}
            //else if (_enemyStateManager.IsPlayerInChaseRange())
            //{
            //    _enemyStateManager.SwitchToState("ChaseState");
            //}
            //else
            //{
            //    _enemyStateManager.SwitchToState("WaitState");
            //}
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
