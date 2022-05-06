using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyAttackedHandler : MonoBehaviour
    {
        private DamageManager _damageManager;
        private EnemyStatisticManager _enemyStatisticManager;

        void Start()
        {
            _enemyStatisticManager = GetComponent<EnemyStatisticManager>();
        }

        void OnTriggerEnter(Collider p_collider)
        {
            if (p_collider.tag == "PlayerAttack") //collide with enemy attack's collider which has this tag
            {
                Debug.Log("attacked");

                var _damageManager = p_collider.GetComponent<DamageManager>();

                if (_damageManager)
                {
                    _enemyStatisticManager.DecreaseHealth(_damageManager.GetDamage());
                    Debug.Log("received " + _damageManager.GetDamage() + " dmg");
                }
                else 
                { 
                    Debug.Log("Damage not assigned to attack source."); 
                }

                //Debug.Log("current HP: " + _playerStatisticManager.GetHealth());
            }
        }
    }
}
