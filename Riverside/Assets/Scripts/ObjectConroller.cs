using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FirstPerson
{
    public class ObjectConroller : MonoBehaviour
    {
        private SelectionManager _selectionManager;

        private GameObject currrentPlaceObject;

        private float mouseWheelRotation;
        public Transform theDest;

        private void Start()
        {
            _selectionManager = GetComponent<SelectionManager>();
        }

        public void ControlObject()
        {
            if (!currrentPlaceObject)
            {
                currrentPlaceObject = _selectionManager.GetObjectInFront();
            }
            else
            {
                currrentPlaceObject.transform.GetComponent<BoxCollider>().enabled = false;
                currrentPlaceObject.transform.GetComponent<Rigidbody>().useGravity = false;
                currrentPlaceObject.transform.position = theDest.position;
                currrentPlaceObject.transform.parent = GameObject.Find("Destination").transform;
            }
        }

        public void DestroyObject() 
        {
            if (currrentPlaceObject)
            {
                currrentPlaceObject.transform.GetComponent<BoxCollider>().enabled = true;
                currrentPlaceObject.GetComponent<Rigidbody>().useGravity = true;
               currrentPlaceObject.transform.parent = GameObject.Find("Greybox").transform;
               currrentPlaceObject = null;
            }
        }

        public void RotateFromMouseWheel(float mouseScrollDelta, float rotateAngle)
        {
            mouseWheelRotation = mouseScrollDelta;
            currrentPlaceObject.transform.Rotate(Vector3.up, mouseWheelRotation * rotateAngle);
        }
    }
}