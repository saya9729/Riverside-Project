using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerSecondaryAttackState : AbstractClass.State
    {
        private PlayerStateManager _playerStateManager;

        [Header("Secondary Attack (Shot)")]
        [SerializeField] private float range = 4;
        [SerializeField] private float maxDamage = 20;
        //[SerializeField] private float attackInterval; 
        [SerializeField] private float reloadTime = 1;
        [SerializeField] private float ammo = 2;
        [SerializeField] private float velocity = 10;

        public GameObject bullet;
        public Transform nozzlePoint;

        private bool hasShot = false;

        public void SecondaryAttack()
        {
            _playerStateManager.playerAnimator.SetInteger("attack", 2);
            StartCoroutine(WaitAnim());
        }

        IEnumerator WaitAnim() //wait animation ready to shoot
        {
            int layer = 0;
            AnimatorStateInfo animState = _playerStateManager.playerAnimator.GetCurrentAnimatorStateInfo(layer);
            float shootLength = animState.normalizedTime % 1;
            yield return new WaitForSeconds(shootLength);

            var shot = Instantiate(bullet, nozzlePoint.position, Quaternion.identity);
            shot.GetComponent<Rigidbody>().velocity = nozzlePoint.right * velocity; //create bullet

            _playerStateManager.SwitchState(_playerStateManager.playerIdleState); //set bullet velocity
        }

        private void Start()
        {
            _playerStateManager = GameObject.Find("PlayerStateManager").GetComponent<PlayerStateManager>(); 
        }

        public override void EnterState()
        {
            SecondaryAttack();

        }

        public override void UpdateState()
        {

        }

        public override void ExitState()
        {
            _playerStateManager.playerAnimator.SetInteger("attack", 0);
            StopAllCoroutines(); // stop all shooting sequences

        }
        public override void PhysicsUpdateState()
        {

        }
    }
}
