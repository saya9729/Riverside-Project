using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace Universal
{
    public class AttackManager : MonoBehaviour
    {
        [SerializeField] private float damage = 15f;

        private AttackBody[] _attackBody;

        void Start()
        {
            _attackBody = GetComponentsInChildren<AttackBody>();
        }

        public void EnableHitbox()
        {
            foreach (AttackBody body in _attackBody)
            {
                body.EnableHitbox();
            }
        }
        public void DisableHitbox()
        {
            try
            {
                foreach (AttackBody body in _attackBody)
                {
                    body.DisableHitbox();
                }
            }
            catch
            {
                Start();
                foreach (AttackBody body in _attackBody)
                {
                    body.DisableHitbox();
                }
            }
        }

        public float DealDamage()
        {
            return damage;
        }
    }
}
