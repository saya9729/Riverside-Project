using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class RagdollBody : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private Collider _collider;
        private CharacterJoint _characterJoint;

        private Rigidbody _connectedBody;
        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();

            try
            {
                SetupJoint();
            }
            catch
            {
                
            }            
        }

        private void SetupJoint()
        {
            _characterJoint = GetComponent<CharacterJoint>();
            _connectedBody = _characterJoint.connectedBody;
        }

        public void EnableRagdoll()
        {
            _rigidbody.isKinematic = false;
            _collider.enabled = true;
            try
            {
               _characterJoint.connectedBody = _connectedBody;
            }
            catch
            {
                
            }
        }
        public void DisableRagdoll()
        {
            try 
            {
                _rigidbody.isKinematic = true;
                _collider.enabled = false;                
            }
            catch
            {
                Start();
                _rigidbody.isKinematic = true;
                _collider.enabled = false;                
            }
            try
            {
                _characterJoint.connectedBody = null;
            }
            catch
            {

            }
        }
    }
}