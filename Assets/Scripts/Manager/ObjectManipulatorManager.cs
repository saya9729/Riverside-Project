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
        [SerializeField] private Transform destinationParent;

        private GameObject _currentPickUpObject;
        private Transform _originalParent;
        
        public void PickUpObject(GameObject p_currentSelectObject)
        {
            _currentPickUpObject = p_currentSelectObject;
            _currentPickUpObject.GetComponent<BoxCollider>().enabled = false;
            //_currentPickUpObject.GetComponent<Rigidbody>().useGravity = false;
            _currentPickUpObject.transform.position = pickUpDestination.position;
            _originalParent = _currentPickUpObject.transform.parent;
            _currentPickUpObject.transform.parent = destinationParent;
            //use later
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

        public void UpdateObjectPosition()
        {
            _currentPickUpObject.transform.position = pickUpDestination.position;
        }

        public void DropObject() 
        {
            _currentPickUpObject.GetComponent<BoxCollider>().enabled = true;
            //_currentPickUpObject.GetComponent<Rigidbody>().useGravity = true;
            _currentPickUpObject.transform.parent = _originalParent;
            //_currentPickUpObject = null;
            //use later
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
            //use later
            //if (_currentPickUpObject != null)
            //{
            //    _mouseWheelRotation = p_mouseScrollDelta * rotateAngle;
            //    _currentPickUpObject.transform.Rotate(Vector3.up, _mouseWheelRotation);
            //}
        }
    }
}