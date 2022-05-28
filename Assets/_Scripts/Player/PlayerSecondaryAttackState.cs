using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerSecondaryAttackState : AbstractClass.State
    {
        private PlayerStateManager _playerStateManager;
        public GunRecoil gunRecoil;

        [Header("Secondary Attack (Shot)")]
        [SerializeField] private float range = 4;
        [SerializeField] private float maxDamage = 20;
        //[SerializeField] private float attackInterval; 
        [SerializeField] private float reloadTime = 1;
        [SerializeField] private float ammo = 2;
        [SerializeField] private float velocity = 10;

        private Camera _cam;

        private bool hasShot = false;

        public void SecondaryAttack()
        {
            //_playerStateManager.playerAnimator.SetInteger("attack", 2);
            gunRecoil.Fire();
            FindObjectOfType<AudioManager>().Play("shoot");
            StartCoroutine(WaitAnim());
        }

        void Shoot()
        {
            RaycastHit hit;
            if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, range))
            {
                Debug.Log("hit " + hit.transform.name);
                Transform target = hit.transform;

                if (target.CompareTag("Enemy"))
                {
                    //decrease enemy HP
                }

            }
        }

        IEnumerator WaitAnim() //wait animation ready to shoot
        {
            int layer = 0;
            AnimatorStateInfo animState = _playerStateManager.playerAnimator.GetCurrentAnimatorStateInfo(layer);
            float shootLength = animState.normalizedTime % 1;
            yield return new WaitForSeconds(shootLength);

            // var shot = Instantiate(bullet, nozzlePoint.position, Quaternion.identity);
            // shot.GetComponent<Rigidbody>().velocity = nozzlePoint.right * velocity; //create bullet

            Shoot();

            _playerStateManager.SwitchState(_playerStateManager.playerIdleState);
        }

        private void Start()
        {
            _playerStateManager = GetComponent<PlayerStateManager>();
            _cam = GameObject.Find("MainCamera").GetComponent<Camera>();
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
            //_playerStateManager.playerAnimator.SetInteger("attack", 0);
            StopAllCoroutines(); // stop all shooting sequences

        }
        public override void PhysicsUpdateState()
        {

        }
    }
}
