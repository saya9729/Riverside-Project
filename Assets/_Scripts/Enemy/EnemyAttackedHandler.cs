using UnityEngine;

namespace Enemy
{
    public class EnemyAttackedHandler : MonoBehaviour
    {
        [SerializeField] private string playerAttackHitboxTag = "PlayerAttack";

        private Universal.AttackManager _damageManager;
        private EnemyStateManager _enemyStateManager;

        public GameObject particleSpark;
        public GameObject particleBlood;

        private void Start()
        {
            _enemyStateManager = GetComponentInParent<EnemyStateManager>();
        }

        private void OnCollisionEnter(Collision collisionInfo)
        {
            if (collisionInfo.collider.CompareTag(playerAttackHitboxTag)) //collide with player attack's collider which has this tag
            {
                //Debug.Log("attacked by "+ collisionInfo.gameObject.name);
                ContactPoint hitPoint = collisionInfo.GetContact(0);
                //Vector3 particleDirection = hitPoint2.point - hitPoint1.point;

                Instantiate(particleSpark, hitPoint.point, Quaternion.Euler(hitPoint.normal));
                Instantiate(particleBlood, hitPoint.point, Quaternion.Euler(hitPoint.normal));
                AudioInterface.PlayAudio("enemyHit");

                _damageManager = collisionInfo.collider.gameObject.GetComponentInParent<Universal.AttackManager>();

                if (_damageManager)
                {
                    _enemyStateManager.ReceiveDamage(_damageManager.DealDamage());
                    //Debug.Log("received " + _damageManager.GetDamage() + " dmg");
                }
                else
                {
                    //Debug.Log("Damage not assigned to attack source."); 
                }

                //Debug.Log("current HP: " + _playerStatisticManager.GetHealth());
            }
        }
        private void OnTriggerEnter(Collider p_collider)
        {
            if (p_collider.CompareTag(playerAttackHitboxTag))
            {
                //reset _damageManager
                _damageManager = null;
                _damageManager =p_collider.GetComponentInParent<Universal.AttackManager>();
                try
                {
                    _enemyStateManager.ReceiveDamage(_damageManager.DealDamage());
                }
                catch
                {
                    Debug.Log("Damage not assigned to attack source."); 
                }
            }
        }
    }
}
