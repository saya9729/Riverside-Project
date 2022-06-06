using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Universal
{
    public class AttackBody : MonoBehaviour
    {
        private Collider _collider;

        // Start is called before the first frame update
        void Start()
        {
            _collider = GetComponent<Collider>();
        }

        public void EnableHitbox()
        {
            _collider.enabled = true;
        }
        public void DisableHitbox()
        {
            _collider.enabled = false;
        }
    }
}
