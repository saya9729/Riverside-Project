using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class AttackedPlayerManager : MonoBehaviour
    {
        private DamageManager _damageManager;
        private PlayerStatisticManager _playerStatisticManager;

        void Start()
        {
            _playerStatisticManager = GetComponentInChildren<PlayerStatisticManager>();
                
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "EnemyAttack") //collide with enemy attack's collider which has this tag
            {
                Debug.Log("attacked");

                var _damageManager = other.GetComponent<DamageManager>();

                if (_damageManager)
                {
                    _playerStatisticManager.DecreaseHealth(_damageManager.GetDamage());
                    Debug.Log("received " + _damageManager.GetDamage() + " dmg");
                }
                else Debug.Log("Damage not assigned to attack source.");

                //Debug.Log("current HP: " + _playerStatisticManager.GetHealth());
            }
        }
    }
}
