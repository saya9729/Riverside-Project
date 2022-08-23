using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEnvironment
{
    public class SnapPlayerToPlatform : MonoBehaviour
    {
        private Transform _playerOldParrent;
        private void Start()
        {
            _playerOldParrent = GameObject.FindGameObjectWithTag("Player").transform.parent;
        }
        private void OnTriggerEnter(Collider p_other)
        {
            if (p_other.CompareTag("Player"))
            {
                p_other.gameObject.transform.SetParent(transform);
            }
        }
        private void OnTriggerExit(Collider p_other)
        {
            if (p_other.CompareTag("Player"))
            {
                p_other.gameObject.transform.SetParent(_playerOldParrent);
            }
        }
    }
}
