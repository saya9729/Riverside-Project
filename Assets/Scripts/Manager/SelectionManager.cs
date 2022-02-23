using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class SelectionManager : MonoBehaviour
    {
        //Variables
        [SerializeField] private float selectableRadius = 20.0f;

        private PlayerStateManager _playerStateManager;

        private void Start()
        {
            _playerStateManager = GameObject.Find("PlayerStateManager").GetComponent<PlayerStateManager>();
        }

        public GameObject GetObjectAtScreenCenter()
        {
            var ray = Camera.main.ScreenPointToRay(_playerStateManager.inputManager.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.distance < selectableRadius)
                {
                    return hit.transform.gameObject;
                }
            }
            return null;
        }
    }
}