using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class ObjectManipulatorManager : MonoBehaviour
    {
        [Header("Object")]
        [Tooltip("Set Rotation speed for object")]
        [SerializeField] private float rotateAngle = 10.0f;
        [SerializeField] private Transform pickUpDestination;

        private GameObject _currentPickUpObject;
        
        public void PickUpObject(GameObject p_currentSelectObject)
        {
            _currentPickUpObject = p_currentSelectObject;
            _currentPickUpObject.GetComponent<BoxCollider>().enabled = false;
            _currentPickUpObject.GetComponent<Rigidbody>().useGravity = false;
            _currentPickUpObject.transform.position = pickUpDestination.position;
            _currentPickUpObject.transform.parent = GameObject.Find("Destination").transform;

            //if (_currentPickUpObject==null)
            //{
            //    _currentPickUpObject = p_currentSelectObject;
            //}
            //else
            //{
            //    _currentPickUpObject.GetComponent<BoxCollider>().enabled = false;
            //    _currentPickUpObject.GetComponent<Rigidbody>().useGravity = false;
            //    _currentPickUpObject.transform.position = pickUpDestination.position;
            //    _currentPickUpObject.transform.parent = GameObject.Find("Destination").transform;
            //}
        }

        public void DropObject() 
        {
            _currentPickUpObject.GetComponent<BoxCollider>().enabled = true;
            _currentPickUpObject.GetComponent<Rigidbody>().useGravity = true;
            _currentPickUpObject.transform.parent = GameObject.Find("Greybox").transform;
            _currentPickUpObject = null;

            //if (_currentPickUpObject!=null)
            //{
            //    _currentPickUpObject.GetComponent<BoxCollider>().enabled = true;
            //    _currentPickUpObject.GetComponent<Rigidbody>().useGravity = true;
            //    _currentPickUpObject.transform.parent = GameObject.Find("Greybox").transform;
            //    _currentPickUpObject = null;
            //}
        }

        public void RotateFromMouseWheel(float p_mouseScrollDelta)
        {
            _currentPickUpObject.transform.Rotate(Vector3.up, p_mouseScrollDelta * rotateAngle);

            //if (_currentPickUpObject != null)
            //{
            //    _mouseWheelRotation = p_mouseScrollDelta * rotateAngle;
            //    _currentPickUpObject.transform.Rotate(Vector3.up, _mouseWheelRotation);
            //}
        }
    }
}