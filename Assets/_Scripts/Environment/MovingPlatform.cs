using UnityEngine;

namespace GameEnvironment
{
    public class MovingPlatform : MonoBehaviour
    {
        [System.Serializable]
        private class PlatformWaypoint
        {
            public Transform waypoint;
            public float timeToNextWaypoint = 2.5f;
        }
        
        [SerializeField]  PlatformWaypoint[] platformWaypoint;
        private int _currentWaypointIndex = 0;
      

        private float _timeElapsed = 0f;
        private float _distance = 1f;
        
        private void Update()
        {
            MovePlatformToWaypoint();
        }
        private void MovePlatformToWaypoint()
        {
            if (Vector3.Distance(transform.position, platformWaypoint[_currentWaypointIndex].waypoint.transform.position) < 0.1f)
            {
                _currentWaypointIndex++;
                if (_currentWaypointIndex >= platformWaypoint.Length)
                {
                    _currentWaypointIndex = 0;
                }

                _distance = Vector3.Distance(transform.position, platformWaypoint[_currentWaypointIndex].waypoint.transform.position);
                _timeElapsed = 0;
            }

            transform.position = Vector3.MoveTowards(
                transform.position, 
                platformWaypoint[_currentWaypointIndex].waypoint.transform.position, 
                Universal.Smoothing.SineWaveSmooth(
                    _distance / 2, _timeElapsed, 
                    platformWaypoint[_currentWaypointIndex].timeToNextWaypoint * 2) * Time.deltaTime);
            
            _timeElapsed += Time.deltaTime;

            //transform.position = Vector3.MoveTowards(transform.position, waypoints[_currentWaypointIndex].transform.position, movingPlatformSpeed * Time.deltaTime);
        }
    }
    
}