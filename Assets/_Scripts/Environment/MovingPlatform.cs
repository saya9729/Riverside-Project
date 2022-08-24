using UnityEngine;

namespace GameEnvironment
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovingPlatform : MonoBehaviour
    {
        [System.Serializable]
        private class PlatformWaypoint
        {
            public Transform waypoint;
            public float timeToThisWaypoint = 2.5f;
        }

        [SerializeField] private PlatformWaypoint[] platformWaypoint;
        private int _currentWaypointIndex = 0;


        private float _timeElapsed = 0f;
        private float _distance = 1f;
        private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            //_rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
            _rigidbody.freezeRotation = true;
        }

        private void FixedUpdate()
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
            else
            {
                _timeElapsed += Time.deltaTime;
            }
            if (platformWaypoint[_currentWaypointIndex].timeToThisWaypoint == 0)
            {
                transform.position = platformWaypoint[_currentWaypointIndex].waypoint.transform.position;
            }
            else
            {
                _rigidbody.velocity = (platformWaypoint[_currentWaypointIndex].waypoint.transform.position - transform.position).normalized
                    * Universal.Smoothing.SineWaveSmooth(
                        _distance / 2, _timeElapsed,
                        platformWaypoint[_currentWaypointIndex].timeToThisWaypoint * 2);
                //transform.position = Vector3.MoveTowards(
                //    transform.position,
                //    platformWaypoint[_currentWaypointIndex].waypoint.transform.position,
                //    Universal.Smoothing.SineWaveSmooth(
                //        _distance / 2, _timeElapsed,
                //        platformWaypoint[_currentWaypointIndex].timeToThisWaypoint * 2) * Time.deltaTime);
            }
            //transform.position = Vector3.MoveTowards(transform.position, waypoints[_currentWaypointIndex].transform.position, movingPlatformSpeed * Time.deltaTime);
        }
    }

}