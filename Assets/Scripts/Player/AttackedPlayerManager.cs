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
            _playerStatisticManager = GetComponent<PlayerStatisticManager>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "EnemyAttack") //collide with enemy attack's collider which has this tag
            {
                Debug.Log("attacked");

                var dmgManager = other.GetComponent<DamageManager>();

                if (dmgManager)
                {
                    _playerStatisticManager.DecreaseHealth(dmgManager.GetDamage());
                    Debug.Log("received " + dmgManager.GetDamage() + " dmg");
                }
                else Debug.Log("Damage not assigned to attack source.");

                Debug.Log("current HP: " + _playerStatisticManager.GetHealth());
            }
        }
    }
}
