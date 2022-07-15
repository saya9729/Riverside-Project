using UnityEngine;
using System.Collections;

namespace Player
{
    public class HeadBob : MonoBehaviour
    {
        private PlayerMovementStateManager _playerMovementStateManager;

        [SerializeField] private bool _enable = true;

        [SerializeField, Range(0, 100f)] private float _amplitude = 5.0f;
        [SerializeField, Range(0, 30)] private float _frequency = 20.0f;
        [SerializeField, Range(0, 10)] private float _resetSpeed = 10.0f;
        [SerializeField, Range(0, 10)] private float _jumpBobDuration = 0.5f;
        [SerializeField, Range(0, 1000)] private float _jumpBobSpeed = 10f;

        [SerializeField] private Transform _camera = null;
        [SerializeField] private Transform _cameraHolder = null;

        private float _tempRotX, _tempRotZ;
        private Quaternion _tempRot;
        private bool _isCRRunning = false;
        private bool _isJumping = false;
        private bool _isFinishedReset = true;

        private void PlayMotion(Quaternion motion)
        {
            _camera.localRotation = motion;
        }

        private void CheckMotion()
        {
            if (_playerMovementStateManager.inputManager.move != Vector2.zero && _playerMovementStateManager.isGrounded)
            {
                PlayMotion(FootStepMotion());
            }
        }

        private void CheckJump()
        {
            if (_isJumping)
            {
                PlayMotion(JumpMotion());
            }

            if (_playerMovementStateManager.inputManager.jump && _playerMovementStateManager.isGrounded)
            {
                _isFinishedReset = false;
                _tempRot = _camera.localRotation;
                StartCoroutine(JumpTrigger());
            }

            if (_playerMovementStateManager.inputManager.jump && !_playerMovementStateManager.isGrounded)
            {
                _isFinishedReset = false;
                if (_tempRot == Quaternion.Euler(0, 0, 0))
                {
                    _tempRot = _camera.localRotation;
                }
                if (_isCRRunning)
                {
                    StopAllCoroutines();
                }
                StartCoroutine(JumpTrigger());
            }
        }

        private void ResetRotation()
        {
            if (_playerMovementStateManager.inputManager.move != Vector2.zero)
            {
                return;
            }
            if (_camera.localRotation.z == 0f) 
            {
                return;
            }

            _tempRotZ = Mathf.Lerp(_tempRotZ, 0, _resetSpeed * Time.deltaTime);
            _camera.localRotation = Quaternion.Euler(_camera.localRotation.x, _camera.localRotation.y, _tempRotZ);
            //Debug.Log("resettingwalk");
        }

        private void ResetJump()
        {
            if (_camera.localRotation.x <= _tempRot.x)
            {
                if (!_isFinishedReset)
                {
                    _isFinishedReset = true;
                }
            }
            if (_isFinishedReset)
            {
                return;
            }

            _tempRotX = _camera.localRotation.x;
            _tempRotX -= Time.deltaTime * _jumpBobSpeed;
            _camera.localRotation = Quaternion.Euler(_tempRotX, _camera.localRotation.y, _camera.localRotation.z);
            //Debug.Log("resettingJump");
        }

        private void Awake()
        {
            _tempRot = _camera.localRotation;
            _playerMovementStateManager = GetComponent<PlayerMovementStateManager>();
        }

        private Quaternion FootStepMotion()
        {
            Quaternion rot = Quaternion.Euler(_camera.localRotation.x, _camera.localRotation.y, 0);
            rot.z += Mathf.Cos(Time.time * _frequency / 2) * _amplitude * 2 / 10000;
            _tempRotZ = rot.z;
            return rot;
        }

        private Quaternion JumpMotion()
        {
            Quaternion rot = Quaternion.Euler(_camera.localRotation.x, _camera.localRotation.y, _camera.localRotation.z);
            rot.x += Time.deltaTime * _jumpBobSpeed;
            //Debug.Log("jumping");
            // Debug.Log(_camera.localRotation.x + " " + rot.x);
            return rot;
        }

        private void Update()
        {
            if (!_enable) 
            {
                return;
            }
            CheckMotion();
            //CheckJump(); //bug vl
            ResetRotation();
            //ResetJump();
        }

        private IEnumerator JumpTrigger()
        {
            //Debug.Log("coroutine");
            _isCRRunning = true;
            _isJumping = true;

            yield return new WaitForSeconds(_jumpBobDuration);

            _isJumping = false;
            _isCRRunning = false;
        }
    }
}
