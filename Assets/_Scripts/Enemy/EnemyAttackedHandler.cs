using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(EnemyStatisticManager))]
    public class EnemyAttackedHandler : MonoBehaviour
    {
        [SerializeField] private string playerAttackHitboxTag = "PlayerAttack";

        private Universal.AttackManager _damageManager;
        private EnemyStatisticManager _enemyStatisticManager;

        public GameObject particleSpark;

        void Start()
        {
            _enemyStatisticManager = GetComponent<EnemyStatisticManager>();
        }

        void OnCollisionEnter(Collision collisionInfo)
        {
            if (collisionInfo.collider.CompareTag(playerAttackHitboxTag)) //collide with player attack's collider which has this tag
            {
                Debug.Log("attacked by "+ collisionInfo.gameObject.name);
                ContactPoint hitPoint = collisionInfo.GetContact(0);
                //Vector3 particleDirection = hitPoint2.point - hitPoint1.point;

                Instantiate(particleSpark, hitPoint.point, Quaternion.Euler(hitPoint.normal));

                _damageManager = collisionInfo.collider.gameObject.GetComponentInParent<Universal.AttackManager>();

                if (_damageManager)
                {
                    _enemyStatisticManager.DecreaseHealth(_damageManager.DealDamage());
                    //Debug.Log("received " + _damageManager.GetDamage() + " dmg");
                }
                else 
                { 
                    //Debug.Log("Damage not assigned to attack source."); 
                }

                //Debug.Log("current HP: " + _playerStatisticManager.GetHealth());
            }
        }
    }
}
