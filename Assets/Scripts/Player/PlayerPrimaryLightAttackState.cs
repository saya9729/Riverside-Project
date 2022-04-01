using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerPrimaryLightAttackState : AbstractClass.State
    {
        private PlayerStateManager _playerStateManager;
        private Animator _anim;

        [Header("Primary Attack (Slash)")]
        //[SerializeField] private float range;
        [SerializeField] private float maxDamage = 10;
        [SerializeField] private float attackInterval = 0.5f;
        //[SerializeField] private float reloadTime;
        //[SerializeField] private float ammo;

        public void PrimaryAttack()
        {
            _anim.SetInteger("attack", 1);
            //toggle weapon collider
        }

        private void Start()
        {
            _playerStateManager = GameObject.Find("PlayerStateManager").GetComponent<PlayerStateManager>();
            _anim = gameObject.transform.parent.GetComponent<Animator>();
        }

        public override void EnterState()
        {
            Debug.Log("enter primary light attack state");
            PrimaryAttack();

        }

        public override void UpdateState()
        {

        }

        public override void ExitState()
        {
            Debug.Log("exit primary light attack state");
            _anim.SetInteger("attack", 0); //return to idle

        }
        public override void PhysicsUpdateState()
        {

        }
    }
}
