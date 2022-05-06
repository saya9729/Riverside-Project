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

        private InputManager _inputManager;

        [SerializeField] private float AimSpeed = 10;

        private bool isAiming = false;
        private bool isReloading = false;

        private Transform anchor;
        public Transform state_ads;
        public Transform state_hip;

        void Start()
        {
            _inputManager = FindObjectOfType<InputManager>();
        }

        void Update()
        {
            if(_inputManager.switchAim)
            {
                Debug.Log("to ads");
                //hip
                transform.position = Vector3.Lerp(transform.position, state_ads.position, Time.deltaTime * AimSpeed);
                transform.rotation = Quaternion.Lerp(transform.rotation, state_ads.rotation, Time.deltaTime * AimSpeed);
                //isAiming = true; 
            }
            else
            {
                Debug.Log("to hip");
                //aim
                transform.position = Vector3.Lerp(transform.position, state_hip.position, Time.deltaTime * AimSpeed);
                transform.rotation = Quaternion.Lerp(transform.rotation, state_hip.rotation, Time.deltaTime * AimSpeed);
                //isAiming = false;  
            }
        }

        public void Aim()
        {
            if (isReloading) return;

            if (isAiming)
            {
                Debug.Log("to hip");
                //aim
                transform.position = Vector3.Lerp(transform.position, state_ads.position, Time.deltaTime * AimSpeed);
                isAiming = false;            
            }
            else
            {
                Debug.Log("to ads");
                //hip
                transform.position = Vector3.Lerp(transform.position, state_hip.position, Time.deltaTime * AimSpeed);
                isAiming = true; 
            }
        }
    }
}
