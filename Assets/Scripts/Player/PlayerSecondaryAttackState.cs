using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerSecondaryAttackState : AbstractClass.State
    {
        private PlayerStateManager _playerStateManager;
        private Animator _anim;

        [Header("Secondary Attack (Shot)")]
        [SerializeField] private float range = 4;
        [SerializeField] private float maxDamage = 20;
        //[SerializeField] private float attackInterval; 
        [SerializeField] private float reloadTime = 1;
        [SerializeField] private float ammo = 2;
        [SerializeField] private float velocity = 10;
        [SerializeField] private float delay = 1.2f;

        public GameObject bullet;
        public Transform nozzlePoint;

        private bool hasShot = false;

        public void SecondaryAttack()
        {
            _anim.SetInteger("attack", 2);
            StartCoroutine(WaitAnim(delay));
        }

        IEnumerator WaitAnim(float time) //wait animation ready to shoot
        {
            yield return new WaitForSeconds(time);

            var shot = Instantiate(bullet, nozzlePoint.position, Quaternion.identity);
            shot.GetComponent<Rigidbody>().velocity = nozzlePoint.right * velocity;
        }

        private void Start()
        {
            _playerStateManager = GameObject.Find("PlayerStateManager").GetComponent<PlayerStateManager>(); //create bullet
            _anim = gameObject.transform.parent.GetComponent<Animator>(); //set velocity to bullet
        }

        // private void Update() {
        //     if(_anim.GetInteger("attack") == 2 && !hasShot)
        //     {
        //         StartCoroutine(WaitAnim(delay));
        //         hasShot = true;
        //     }
        // }

        public override void EnterState()
        {
            Debug.Log("enter secondary attack state");
            SecondaryAttack();

        }

        public override void UpdateState()
        {

        }

        public override void ExitState()
        {
            Debug.Log("exit secondary attack state");
            _anim.SetInteger("attack", 0);
            StopAllCoroutines(); // stop all shooting sequences

        }
        public override void PhysicsUpdateState()
        {

        }
    }
}
