using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class AimManager : MonoBehaviour
    {
        //ref: https://github.com/Kawaiisun/SimpleHostile/blob/master/Simple%20Hostile/Assets/Scripts/Weapon.cs

        private InputManager inputManager;

        [SerializeField] private float aimSpeed = 10;
        [SerializeField] private float hipGunDifference = 0.0001f;

        private bool isReloading = false;

        private Transform anchor;
        public Transform stateAds;
        public Transform stateHip;

        void Start()
        {
            inputManager = FindObjectOfType<InputManager>();
        }

        void Update()
        {
            if(inputManager.switchAim)
            {
                Debug.Log("to ads");
                //hip
                transform.position = Vector3.Lerp(transform.position, stateAds.position, Time.deltaTime * aimSpeed);
                transform.rotation = Quaternion.Lerp(transform.rotation, stateAds.rotation, Time.deltaTime * aimSpeed);
            }
            else if((stateHip.position - transform.position).magnitude >= hipGunDifference) //check if gun returned to hip position
            {
                Debug.Log("to hip");
                //aim
                transform.position = Vector3.Lerp(transform.position, stateHip.position, Time.deltaTime * aimSpeed);
                transform.rotation = Quaternion.Lerp(transform.rotation, stateHip.rotation, Time.deltaTime * aimSpeed);
            }
        }
    }
}
