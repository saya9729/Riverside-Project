using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    [RequireComponent(typeof(PlayerStatisticManager))]
    public class PlayerAttackedManager : MonoBehaviour
    {
        [SerializeField] private string enemyAttackHitboxTag = "EnemyAttack";

        private Universal.AttackManager _damageManager;
        private PlayerStatisticManager _playerStatisticManager;

        [SerializeField] private float hitDuration = 1.5f;

        public GameObject PlayerHit;

        void Start()
        {
            _playerStatisticManager = GetComponentInChildren<PlayerStatisticManager>();

        }

        void OnTriggerEnter(Collider p_collider)
        {
            if (p_collider.CompareTag(enemyAttackHitboxTag)) //collide with enemy attack's collider which has this tag
            {
                Debug.Log("attacked by "+p_collider.gameObject.name);
                AudioInterface.PlayAudio("playerHit");
                if (PlayerHit && PlayerHit.GetComponent<Image>())
                {
                    if (!PlayerHit.GetComponent<Image>().enabled)
                    {
                        PlayerHit.GetComponent<Image>().enabled = true;
                    }
                    else
                    {
                        StopAllCoroutines();
                    }
                    StartCoroutine(WaitDisplayHitEffect());
                }

                _damageManager = p_collider.GetComponentInParent<Universal.AttackManager>();

                if (_damageManager)
                {
                    _playerStatisticManager.DecreaseHealth(_damageManager.DealDamage());
                    //Debug.Log("received " + _damageManager.GetDamage() + " dmg");
                }
                else
                {
                    //Debug.Log("Damage not assigned to attack source.");
                }

                //Debug.Log("current HP: " + _playerStatisticManager.GetHealth());
            }
        }

        IEnumerator WaitDisplayHitEffect()
        {
            yield return new WaitForSeconds(hitDuration);

            PlayerHit.GetComponent<Image>().enabled = false;
        }
    }
}
