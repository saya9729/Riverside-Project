using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class TimeBody : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private Vector3 _recordedVelocity;
        private Vector3 _recordedAngularVelocity;

        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }        

        public void StopTime()
        {
            _recordedVelocity = _rigidbody.velocity;
            _recordedAngularVelocity = _rigidbody.angularVelocity;

            _rigidbody.velocity = Vector3.zero; //makes the rigidbody stop moving
            _rigidbody.angularVelocity = Vector3.zero;

            _rigidbody.isKinematic = true; //not affected by forces
        }
        public void ContinueTime()
        {
            _rigidbody.isKinematic = false;

            _rigidbody.velocity = _recordedVelocity; //Adds back the recorded velocity when time continues
            _rigidbody.angularVelocity = _recordedAngularVelocity;
        }
    }
}
