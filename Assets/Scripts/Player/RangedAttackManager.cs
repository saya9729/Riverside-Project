using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class RangedAttackManager : MonoBehaviour
    {
        private PlayerStateManager _input;
        private Camera _cam;
        private float range = 1000f;
        private float dmg = 10f;

        void Start()
        {
            _cam = GameObject.Find("MainCamera").GetComponent<Camera>();
            _input = FindObjectOfType<PlayerStateManager>();
        }

        void Update()
        {
            if (_input.inputManager.secondaryAttack)
            {
                Shoot();
            }
        }

        void Shoot()
        {
            RaycastHit hit;
            if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, range))
            {
                Debug.Log("hit " + hit.transform.name);
                Transform target = hit.transform;

                if (target.tag == "Player")
                {
                    target.GetComponent<PlayerStatisticManager>().DecreaseHealth(dmg);
                }
                if (target.tag == "Enemy")
                {
                    //decrease enemy HP
                }

            }
        }
    }
}

