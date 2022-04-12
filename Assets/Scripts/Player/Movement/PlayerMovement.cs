using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 4.0f;
        [Tooltip("Run speed of the character in m/s")]
        public float RunSpeed = 6.0f;
        [Tooltip("Rotation speed of the character")]
        public float RotationSpeed = 1.0f;
        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        [Space(10)]
        [Tooltip("Dash speed of the character in m/s")]
        public float DodgeSpeed = 2f;
        [Tooltip("Dashing duration of each dash")]
        public float DodgeDuration = 0.25f;
        [Tooltip("Time required to pass before being able to dash again")]
        public float DodgeTimeout = 1f;


        [Space(10)]
        [Tooltip("Crouch speed of the character in m/s")]
        public float CrouchSpeed = 2f;
        [Tooltip("Crouch height of character")]
        public float CrouchHeight = 1.6f;
        public Vector3 CrouchCenter = new Vector3(0, 0.93f, 0);
        public float timeToCrouch = 0.25f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;
        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.1f;
        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Roofed")]
        public bool RoofedFlag = true;
        public float RoofedOffset = 1.94f;
        public float RoofedDistance = 0.22f;
        public LayerMask RoofedLayers;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;
        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;
        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.5f;
        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;
        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 90.0f;
        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -90.0f;

        //input direction
        private Vector3 _inputDirection;
        //controller original value
        private float _controllerOriginalHeight;
        private Vector3 _controllerOriginalCenter;
        private bool _isCrouching;

        // cinemachine
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout unscaledDeltaTime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;
        private float _dodgeTimeoutDelta;

        private PlayerInput _playerInput;
        private CharacterController _controller;
        private Rigidbody _playerRigidbody;
        private InputManager _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool IsCurrentDeviceMouse => _playerInput.currentControlScheme == "KeyboardMouse";

        private void Awake()
        {
            // get a reference to our main camera
            //if (_mainCamera == null)
            //{
            //    _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            //}
        }

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<InputManager>();
            _playerInput = GetComponent<PlayerInput>();
            _playerRigidbody = GetComponent<Rigidbody>();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
            _dodgeTimeoutDelta = DodgeTimeout;

            //set original controller value
            _controllerOriginalHeight = _controller.height;
            _controllerOriginalCenter = _controller.center;
        }

        private void Update()
        {
            JumpAndGravity();
            GroundedCheck();
            RoofCheck();
            Move();
            Crouch();
            Dodge();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
        }

        private void RoofCheck()
        {
            // set sphere position, with offset
            Vector3 rayPosition = new Vector3(transform.position.x, transform.position.y + RoofedOffset, transform.position.z);
            RoofedFlag = Physics.Raycast(rayPosition, Vector3.up, RoofedDistance, RoofedLayers, QueryTriggerInteraction.Ignore);
        }

        private void CameraRotation()
        {
            // if there is an input
            if (_input.look.sqrMagnitude >= _threshold)
            {
                //Don't multiply mouse input by Time.unscaledDeltaTime
                //float unscaledDeltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.unscaledDeltaTime;
                float unscaledDeltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.unscaledDeltaTime;

                _cinemachineTargetPitch += _input.look.y * RotationSpeed * unscaledDeltaTimeMultiplier;
                _rotationVelocity = _input.look.x * RotationSpeed * unscaledDeltaTimeMultiplier;

                // clamp our pitch rotation
                _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

                // Update Cinemachine camera target pitch
                CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

                // rotate the player left and right
                transform.Rotate(Vector3.up * _rotationVelocity);
            }
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _isCrouching ? CrouchSpeed : RunSpeed;
            //float targetSpeed = RunSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            //// a reference to the players current horizontal velocity
            //float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
            ////float currentHorizontalSpeed = new Vector3(_playerRigidbody.velocity.x, 0.0f, _playerRigidbody.velocity.z).magnitude;

            //float speedOffset = 0.1f;
            //float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            //// accelerate or decelerate to target speed
            //if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            //{
            //    // creates curved result rather than a linear one giving a more organic speed change
            //    // note T in Lerp is clamped, so we don't need to clamp our speed
            //    _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.unscaledDeltaTime * SpeedChangeRate);

            //    // round speed to 3 decimal places
            //    _speed = Mathf.Round(_speed * 1000f) / 1000f;
            //}
            //else
            //{
            _speed = targetSpeed;
            //}
            //_speed = targetSpeed;
            // normalise input direction
            _inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero)
            {
                // move
                _inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
            }

            // move the player
            _controller.Move(_inputDirection.normalized * (_speed * Time.unscaledDeltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.unscaledDeltaTime);
            //_playerRigidbody.AddForce(inputDirection.normalized * (_speed * Time.unscaledDeltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.unscaledDeltaTime);
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                    //Debug.Log("Grounded" + _verticalVelocity);
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height

                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                    //Debug.Log("Jump" + _verticalVelocity);
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.unscaledDeltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.unscaledDeltaTime;
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }
            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.unscaledDeltaTime;
            }
        }

        private void Dodge()
        {
            if (_dodgeTimeoutDelta >= 0.0f)
            {
                _dodgeTimeoutDelta -= Time.unscaledDeltaTime;
                // Debug.Log(_dashTimeoutDelta);
            }
            if (_input.dodge)
            {
                //Debug.Log("dash" + _dashTimeoutDelta);
                if (_dodgeTimeoutDelta <= 0.0f)
                {
                    StartCoroutine(DodgeHandling());
                }
            }
        }

        private IEnumerator DodgeHandling()
        {
            float startTime = Time.unscaledTime;
            //Vector3 dashDirection = new Vector3(0, 0, _input.move.y);
            while (Time.unscaledTime < startTime + DodgeDuration)
            {
                _controller.Move(_inputDirection.normalized * DodgeSpeed * Time.unscaledDeltaTime);
                //turn off player hitbox (i-frame)
                yield return null;
            }
            _dodgeTimeoutDelta = DodgeTimeout;
            _input.dodge = false;
        }

        private void Crouch()
        {
            if (_input.crouch)
            {
                _isCrouching = true;
                _controller.center = CrouchCenter;
                _controller.height = CrouchHeight;
                //StartCoroutine(StandHandling());
                Grounded = false;
            }
            if (!_input.crouch && _isCrouching)
            {
                if (!RoofedFlag)
                {
                    _isCrouching = false;
                    StartCoroutine(StandHandling());
                }
            }
        }

        private IEnumerator StandHandling()
        {
            float timeElapsed = 0;
            float targetHeight = _isCrouching ? CrouchHeight : _controllerOriginalHeight;
            float currentHeight = _controller.height;
            Vector3 targetCenter = _isCrouching ? CrouchCenter : _controllerOriginalCenter;
            Vector3 currentCenter = _controller.center;
            while (timeElapsed < timeToCrouch)
            {
                _controller.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
                _controller.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
                timeElapsed += Time.unscaledDeltaTime;
                yield return null;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;



            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);

            if (RoofedFlag)
            {
                Gizmos.color = transparentGreen;
            }
            else Gizmos.color = transparentRed;

            Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + RoofedOffset, transform.position.z), Vector3.up * RoofedDistance);
        }
    }
}

