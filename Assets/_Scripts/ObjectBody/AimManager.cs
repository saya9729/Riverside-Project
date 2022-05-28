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

        [SerializeField] private float aimSpeed = 10;
        [SerializeField] private float hipGunDifference = 0.0001f;

        private bool isOnCooldown = false;

        private Transform anchor;
        public Transform stateAds;
        public Transform stateHip;
        public Transform stateCD; //cooldown

        void Start()
        {
            _inputManager = FindObjectOfType<InputManager>();
        }

        void Update()
        {
            if (_inputManager.switchAim) //put gun to aim down sight position
            {
                //hip
                FindObjectOfType<AudioManager>().Stop("cooldown");
                isOnCooldown = false;
                transform.position = Vector3.Lerp(transform.position, stateAds.position, Time.deltaTime * aimSpeed);
                transform.rotation = Quaternion.Lerp(transform.rotation, stateAds.rotation, Time.deltaTime * aimSpeed);
            }
            else if (_inputManager.cooldownWeapon) //set gun to cooldown
            {
                Debug.Log("to cooldown");
                if (!FindObjectOfType<AudioManager>().GetSound("cooldown").source.isPlaying)
                {
                    FindObjectOfType<AudioManager>().Play("cooldown");
                }
                isOnCooldown = true;
                transform.position = Vector3.Lerp(transform.position, stateCD.position, Time.deltaTime * aimSpeed);
                transform.rotation = Quaternion.Lerp(transform.rotation, stateCD.rotation, Time.deltaTime * aimSpeed);
            }
            else if ((stateHip.position - transform.position).magnitude >= hipGunDifference) //check if gun returned to hip position and return it to hip
            {
                Debug.Log("to hip");
                FindObjectOfType<AudioManager>().Stop("cooldown");
                isOnCooldown = false;
                transform.position = Vector3.Lerp(transform.position, stateHip.position, Time.deltaTime * aimSpeed);
                transform.rotation = Quaternion.Lerp(transform.rotation, stateHip.rotation, Time.deltaTime * aimSpeed);
            }
        }

        public bool IsOnCooldown()
        {
            return isOnCooldown;
        }
    }
}
