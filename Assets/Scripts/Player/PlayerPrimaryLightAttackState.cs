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

        float _lastTimeCastSkill = 0;
        int _attackTypeIndex = 0;
        int _maxAttackTypeIndex = 1;
        [SerializeField] float maxTimeToNextSkill = 0.5f;

        public void PrimaryAttack()
        {
            
            //toggle weapon collider
            if (Time.unscaledTime - _lastTimeCastSkill < maxTimeToNextSkill)
            {
                if (_attackTypeIndex < _maxAttackTypeIndex)
                    _attackTypeIndex += 1;
                else
                    _attackTypeIndex = 0;
            }
            else
            {
                _attackTypeIndex = 0;
            }
            _playerStateManager.playerAnimator.SetInteger("attackType", _attackTypeIndex);
            _playerStateManager.playerAnimator.SetInteger("attack", 1);

            _lastTimeCastSkill = Time.unscaledTime;
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