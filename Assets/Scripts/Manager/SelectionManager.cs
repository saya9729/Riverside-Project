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
        [NonSerialized] public Ray currentPlayerAim;
        [NonSerialized] public GameObject currentCenterScreenObject;

        private void Start()
        {
            _playerStateManager = GameObject.Find("PlayerStateManager").GetComponent<PlayerStateManager>();
        }

        private void Update()
        {
            currentPlayerAim = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            currentCenterScreenObject = GetObjectAtScreenCenter();
        }

        public GameObject GetObjectAtScreenCenter()
        {
            currentPlayerAim = Camera.main.ScreenPointToRay(_playerStateManager.inputManager.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(currentPlayerAim, out hit))
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