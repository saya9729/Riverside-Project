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
        private PlayerManager _playerManager;

        private void Start()
        {
            _playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        }

        //public void SetPlayerManager(PlayerManager p_playerManager)
        //{
        //    _playerManager = p_playerManager;
        //}

        public GameObject GetObjectAtScreenCenter()
        {
            var ray = Camera.main.ScreenPointToRay(_playerManager.inputManager.mousePosition);
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