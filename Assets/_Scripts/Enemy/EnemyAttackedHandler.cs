using UnityEngine;
using System;

namespace Enemy
{
    public class EnemyAttackedHandler : MonoBehaviour
    {
        [SerializeField] private string playerAttackHitboxTag = "PlayerAttack";

        private Universal.AttackManager _damageManager;
        private EnemyStateManager _enemyStateManager;
        private BloodScreenManager _bloodScreenManager;

        private void Start()
        {
            _enemyStateManager = GetComponentInParent<EnemyStateManager>();
            _bloodScreenManager = FindObjectOfType<BloodScreenManager>();
        }

        private void OnCollisionEnter(Collision collisionInfo)
        {
            if (collisionInfo.collider.CompareTag(playerAttackHitboxTag)) //collide with player attack's collider which has this tag
            {
                //Debug.Log("attacked by "+ collisionInfo.gameObject.name);
                try
                {
                    ContactPoint hitPoint = collisionInfo.GetContact(0);

                    //particle
                    Tuple<VFXID, Vector3, Quaternion> particleSparkInfo = new Tuple<VFXID, Vector3, Quaternion>(VFXID.enemyHitSpark, hitPoint.point, Quaternion.Euler(hitPoint.normal));
                    Tuple<VFXID, Vector3, Quaternion> particleBloodInfo = new Tuple<VFXID, Vector3, Quaternion>(VFXID.enemyHitBlood, hitPoint.point, Quaternion.Euler(hitPoint.normal));
                    this.PostEvent(EventID.onSpawnVFX, particleSparkInfo);
                    this.PostEvent(EventID.onSpawnVFX, particleBloodInfo);

                    //audio
                    this.PostEvent(EventID.onPlaySound, AudioID.enemyHit);
                }
                catch
                {
                    Debug.Log("No contact: "+ collisionInfo.contactCount);                    
                }
                
                try
                {
                    _bloodScreenManager.Play();
                }
                catch
                {
                    Debug.Log("No blood screen manager found");
                }

                try
                {
                    _damageManager = collisionInfo.collider.GetComponentInParent<Universal.AttackManager>();
                    _enemyStateManager.ReceiveDamage(_damageManager.DealDamage());
                    //Debug.Log("received " + _damageManager.DealDamage() + " dmg");
                }
                catch
                {
                    Debug.Log("Damage not assigned to attack source.");
                }

                //Debug.Log("current HP: " + _playerStatisticManager.GetHealth());
            }
        }
        //private void OnTriggerEnter(Collider p_collider)
        //{
        //    if (p_collider.CompareTag(playerAttackHitboxTag))
        //    {
        //        //reset _damageManager
        //        _damageManager = null;
        //        _damageManager =p_collider.GetComponentInParent<Universal.AttackManager>();
        //        try
        //        {
        //            _enemyStateManager.ReceiveDamage(_damageManager.DealDamage());
        //        }
        //        catch
        //        {
        //            Debug.Log("Damage not assigned to attack source."); 
        //        }
        //    }
        //}
    }
}
