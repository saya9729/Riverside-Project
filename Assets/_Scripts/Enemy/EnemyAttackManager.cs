using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace Enemy
{
    public class EnemyAttackManager : MonoBehaviour
    {
        private AttackBody[] _attackBody;
        // Start is called before the first frame update
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
            foreach (AttackBody body in _attackBody)
            {
                body.DisableHitbox();
            }
        }
    }
}
