using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(InputManager))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementController : AbstractClass.StateNew
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
        public Vector3 _inputDirection;
        //controller original value
        private float _controllerOriginalHeight;
        private Vector3 _controllerOriginalCenter;
        public bool _isCrouching;
        public bool _isDashable = true;
        public bool _isJumpable = true;

        // cinemachine
        private float _cinemachineTargetPitch;

        // player
        public float _speed;
        public float _rotationVelocity;
        public float _verticalVelocity;
        public float _terminalVelocity = 53.0f;

        // timeout unscaledDeltaTime

        public PlayerInput _playerInput;
        public CharacterController _controller;
        private Rigidbody _playerRigidbody;
        public InputManager _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool IsCurrentDeviceMouse => _playerInput.currentControlScheme == "KeyboardMouse";

        public float mouseSensitivity;
                
        private PlayerMovementIdleState _playerMovementIdleState;
        private PlayerRunState _playerRunState;
        private PlayerDashState _playerDashState;
        private PlayerIdleWhileAirborneState _playerIdleWhileAirborneState;
        private PlayerRunWhileAirborneState _playerRunWhileAirborneState;
        private PlayerDashWhileAirborneState _playerDashWhileAirborneState;

        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }
        public override void Start()
        {
            InitializeState();
            InitializeComponent();
            InitializeManager();
            InitializeVariable();
        }
        protected override void InitializeState()
        {
            _playerMovementIdleState = GetComponent<PlayerMovementIdleState>();
            _playerRunState = GetComponent<PlayerRunState>();
            _playerDashState = GetComponent<PlayerDashState>();
            _playerIdleWhileAirborneState = GetComponent<PlayerIdleWhileAirborneState>();
            _playerRunWhileAirborneState = GetComponent<PlayerRunWhileAirborneState>();
            _playerDashWhileAirborneState = GetComponent<PlayerDashWhileAirborneState>();

            _playerMovementIdleState.SetSuperState(this);
            _playerRunState.SetSuperState(this);
            _playerDashState.SetSuperState(this);
            _playerIdleWhileAirborneState.SetSuperState(this);
            _playerRunWhileAirborneState.SetSuperState(this);
            _playerDashWhileAirborneState.SetSuperState(this);

            currentSuperState = null;
            currentSubState = _playerMovementIdleState;
            currentSubState.EnterState();
        }
        private void InitializeComponent()
        {
            _controller = GetComponent<CharacterController>();            
            _playerInput = GetComponent<PlayerInput>();
            _playerRigidbody = GetComponent<Rigidbody>();
        }
        protected override void InitializeManager()
        {
            _input = GetComponent<InputManager>();
        }
        private void InitializeVariable()
        {

            //register listener
            this.RegisterListener(EventID.onDodgePress, (param) => onDodgePress());
        }
        private void onDodgePress()
        {
            Debug.Log("event successfully register!");
        }
        private void Update()
        {            
            UpdateAllState();
        }
        protected override void UpdateThisState()
        {
            HandleRunInput();
            Look();
            CheckGrounded();
            Jump();
            ApplyGravity();
        }
        private void FixedUpdate()
        {
            PhysicsUpdateAllState();
        }
        private void LateUpdate()
        {
            Look();
        }

        private void Jump()
        {
            if (Grounded && _input.jump && _isJumpable)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                StartCoroutine(StartJumpCooldown());
            }
        }
        IEnumerator StartJumpCooldown()
        {
            _isJumpable = false;
            yield return new WaitForSeconds(JumpTimeout);
            _isJumpable = true;
        }

        private void HandleRunInput()
        {
            // normalise input direction
            _inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero)
            {
                // move
                _inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
            }
        }        
        
        private void CheckGrounded()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
        }
        
        private void Look()
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
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
        
        private void ApplyGravity()
        {
            if (Grounded)
            {
                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }
            }
            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.unscaledDeltaTime;
            }
            _controller.Move(new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.unscaledDeltaTime);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;



            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
                        
        }

        public override void EnterState()
        {
            throw new NotImplementedException();
        }

        protected override void PhysicsUpdateThisState()
        {
            //HandleRunInput();
            //Look();
            //CheckGrounded();
            //Jump();
            //ApplyGravity();
        }

        public override void ExitState()
        {
            throw new NotImplementedException();
        }

        protected override void CheckSwitchState()
        {
            throw new NotImplementedException();
        }

        public override void SwitchToState(string p_StateType)
        {
            switch (p_StateType)
            {
                case "Idle":
                    SetSubState(_playerMovementIdleState);
                    break;
                case "Run":
                    SetSubState(_playerRunState);
                    break;
                case "Dash":
                    SetSubState(_playerDashState);
                    break;
                case "IdleWhileAirborne":
                    SetSubState(_playerIdleWhileAirborneState);
                    break;
                case "RunWhileAirborne":
                    SetSubState(_playerRunWhileAirborneState);
                    break;
                case "DashWhileAirborne":
                    SetSubState(_playerDashWhileAirborneState);
                    break;
                default:
                    break;
            }
        }

        
    }
}
