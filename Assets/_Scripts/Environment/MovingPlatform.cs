using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEnvironment
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private float movingPlatformSpeed = 1f;
        [SerializeField] private Transform[] waypoints;
        private int _currentWaypointIndex = 0;
        private Transform _playerOldParrent;

        private void Start()
        {
            _playerOldParrent = GameObject.FindGameObjectWithTag("Player").transform.parent;
        }
        void Update()
        {
            MovingPlatformByWaypoint();
        }
        private void MovingPlatformByWaypoint()
        {
            if (Vector3.Distance(transform.position, waypoints[_currentWaypointIndex].transform.position) < 0.1f)
            {
                _currentWaypointIndex++;
                if (_currentWaypointIndex >= waypoints.Length)
                {
                    _currentWaypointIndex = 0;
                }
            }

            transform.position = Vector3.MoveTowards(transform.position, waypoints[_currentWaypointIndex].transform.position, movingPlatformSpeed * Time.deltaTime);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Enter after");
                other.gameObject.transform.SetParent(transform);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Debug.Log("Exit after");
                other.gameObject.transform.SetParent(_playerOldParrent);
            }
        }
    }
}