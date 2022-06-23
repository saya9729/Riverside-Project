using UnityEngine;

namespace Player
{
    public class HeadBob : MonoBehaviour
    {
        private InputManager inputManager;

        [SerializeField] private bool _enable = true;

        [SerializeField, Range(0, 0.1f)] private float _amplitude = 0.015f;
        [SerializeField, Range(0, 30)] private float _frequency = 10.0f;

        [SerializeField] private Transform _camera = null;
        [SerializeField] private Transform _cameraHolder = null;

        public float toggleSpeed = 3.0f;
        private Vector3 _startpos;

        private void PlayMotion(Vector3 motion)
        {
            _camera.localPosition += motion;
        }

        private void CheckMotion()
        {
            if (inputManager.move != Vector2.zero)
            {
                PlayMotion(FootStepMotion());
            }
        }

        private void ResetPosition()
        {
            if (_camera.localPosition == _startpos) return;
            _camera.localPosition = Vector3.Lerp(_camera.localPosition, _startpos, 1 * Time.deltaTime);

        }

        private void Awake()
        {
            _startpos = _camera.localPosition;
            inputManager = GetComponent<InputManager>();
        }

        private Vector3 FootStepMotion()
        {
            Vector3 pos = Vector3.zero;
            pos.y += Mathf.Sin(Time.deltaTime * _frequency) * _amplitude;
            pos.x += Mathf.Cos(Time.deltaTime * _frequency / 2) * _amplitude * 2;
            return pos;
        }

        private void Update()
        {
            if (!_enable) return;
            CheckMotion();
            ResetPosition();
            //_camera.LookAt(FocusTarget());
        }

        private Vector3 FocusTarget()
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + _cameraHolder.localPosition.y, transform.position.z);
            pos += _cameraHolder.forward * 15.0f;
            return pos;
        }
    }
}
