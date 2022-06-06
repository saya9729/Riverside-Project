using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyAttackedHandler : MonoBehaviour
    {
        private DamageManager _damageManager;
        private EnemyStatisticManager _enemyStatisticManager;
        public GameObject particleSpark;

        void Start()
        {
            _enemyStatisticManager = GetComponent<EnemyStatisticManager>();
        }

        void OnCollisionEnter(Collision collisionInfo)
        {
            Debug.Log(collisionInfo.gameObject.name);
            if (collisionInfo.collider.CompareTag("PlayerAttack")) //collide with player attack's collider which has this tag
            {
                Debug.Log("attacked");
                ContactPoint hitPoint = collisionInfo.GetContact(0);
                //Vector3 particleDirection = hitPoint2.point - hitPoint1.point;

                Instantiate(particleSpark, hitPoint.point, Quaternion.Euler(hitPoint.normal));

                _damageManager = collisionInfo.collider.gameObject.GetComponent<DamageManager>();

                if (_damageManager)
                {
                    _enemyStatisticManager.DecreaseHealth(_damageManager.GetDamage());
                    Debug.Log("received " + _damageManager.GetDamage() + " dmg");
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
