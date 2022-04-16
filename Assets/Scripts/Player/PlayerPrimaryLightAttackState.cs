using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerPrimaryLightAttackState : AbstractClass.State
    {
        private PlayerStateManager _playerStateManager;

        [Header("Primary Attack (Slash)")]
        //[SerializeField] private float range;
        [SerializeField] private float maxDamage = 10;
        [SerializeField] private float attackInterval = 0.5f;
        //[SerializeField] private float reloadTime;
        //[SerializeField] private float ammo;

        public void PrimaryAttack()
        {
            _playerStateManager.playerAnimator.SetInteger("attack", 1);
            //toggle weapon collider
        }

        private void Start()
        {
            _playerStateManager = GetComponent<PlayerStateManager>();
        }

        public override void EnterState()
        {
            PrimaryAttack();
            StartCoroutine(WaitAnimation());



        }

        public override void UpdateState()
        {
        }

        public override void ExitState()
        {

        }
        public override void PhysicsUpdateState()
        {

        }
        IEnumerator WaitAnimation()
        {
            int layer = 0;
            AnimatorStateInfo animState = _playerStateManager.playerAnimator.GetCurrentAnimatorStateInfo(layer);
            float attackLength = animState.normalizedTime % 1;
            yield return new WaitForSeconds(attackLength);
            _playerStateManager.SwitchState(_playerStateManager.playerIdleState);
        }
    }
}