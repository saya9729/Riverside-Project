using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class RagdollBody : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void EnableRagdoll()
        {
            _rigidbody.isKinematic = false;
        }
        public void DisableRagdoll()
        {
            try
            {
                _rigidbody.isKinematic = true;
            }
            catch
            {
                Start();
                _rigidbody.isKinematic = true;
            }
        }
    }
}