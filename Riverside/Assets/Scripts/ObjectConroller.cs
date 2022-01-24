using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FirstPerson
{
    public class ObjectConroller : MonoBehaviour
    {
        [Header("Object hold")]
        [Tooltip("This is the object player hold")]
        [SerializeField]
        private GameObject objectPrefab;

        private GameObject currrentPlaceObject;

        private float mouseWheelRotation;
        public Transform theDest;


        public void ControlObject()
        {
            if (!currrentPlaceObject)
            {
                currrentPlaceObject = Instantiate(objectPrefab);
            }
            else
            {
                //currrentPlaceObject.GetComponent<BoxCollider>().enabled = false;
                //currrentPlaceObject.GetComponent<Rigidbody>().useGravity = false;
                currrentPlaceObject.transform.position = theDest.position;
                currrentPlaceObject.transform.parent = GameObject.Find("Destination").transform;
            }
        }

        public void DestroyObject() 
        {
            if (currrentPlaceObject)
            {
               //currrentPlaceObject.GetComponent<Rigidbody>().useGravity = true;
               currrentPlaceObject.transform.parent = null;
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