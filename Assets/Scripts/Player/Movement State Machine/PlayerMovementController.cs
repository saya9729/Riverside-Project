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
        [Tooltip("Run speed of the character in m/s")]
        public float runSpeed = 6.0f;
        [Tooltip("Acceleration and deceleration")]
        public float speedChangeRate = 10.0f;
        [Tooltip("Rotation speed of the character")]
        public float rotationSpeed = 1.0f;

        public float mouseSensitivity=1;

        [Space(10)]
        [Tooltip("Dash speed of the character in m/s")]
        public float dashSpeed = 2f;
        [Tooltip("Dashing duration of each dash")]
        public float dashDuration = 0.25f;
        [Tooltip("Time required to pass before being able to dash again")]
        public float dashTimeout = 1f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float jumpHeight = 1.2f;
        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float gravity = -15.0f;

        public float terminalVelocity = 53.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float jumpCooldown = 0.1f;
        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float airborneCooldown = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool isGrounded = true;
        [Tooltip("Useful for rough ground")]
        public float groundedOffset = -0.14f;
        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float groundedRadius = 0.5f;
        [Tooltip("What layers the character uses as ground")]
        public LayerMask groundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject cinemachineCameraTarget;
        [Tooltip("How far in degrees can you move the camera up")]
        public float topClamp = 90.0f;
        [Tooltip("How far in degrees can you move the camera down")]
        public float bottomClamp = -90.0f;

        //input direction
        public Vector3 inputDirection;    

        // cinemachine
        private float _cinemachineTargetPitch;

        // player stats
        public float speed;
        public float rotationVelocity;
        public float verticalVelocity;
        public bool isDashable = true;
        public bool isJumpable = true;

        private float previousStepOffset;

        // timeout unscaledDeltaTime

        public PlayerInput playerInput;
        public CharacterController characterController;
        public InputManager inputManager;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool IsCurrentDeviceMouse => playerInput.currentControlScheme == "KeyboardMouse";
        
        //States logic
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
        protected override void InitializeComponent()
        {
            characterController = GetComponent<CharacterController>();            
            playerInput = GetComponent<PlayerInput>();
            inputManager = GetComponent<InputManager>();
        }
        
        protected override void InitializeVariable()
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
        public void DisableStepOffset()
        {
            previousStepOffset = characterController.stepOffset;
            characterController.stepOffset = 0;
        }
        public void EnableStepOffset()
        {
            characterController.stepOffset = previousStepOffset;
        }
        private void Jump()
        {
            if (isGrounded && inputManager.jump && isJumpable)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                StartCoroutine(StartJumpCooldown());
            }
        }
        IEnumerator StartJumpCooldown()
        {
            isJumpable = false;
            yield return new WaitForSeconds(jumpCooldown);
            isJumpable = true;
        }

        private void HandleRunInput()
        {
            // normalise input direction
            inputDirection = new Vector3(inputManager.move.x, 0.0f, inputManager.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (inputManager.move != Vector2.zero)
            {
                // move
                inputDirection = transform.right * inputManager.move.x + transform.forward * inputManager.move.y;
            }
        }        
        
        private void CheckGrounded()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
            isGrounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
        }
        
        private void Look()
        {
            // if there is an input
            if (inputManager.look.sqrMagnitude >= _threshold)
            {
                //Don't multiply mouse input by Time.unscaledDeltaTime
                //float unscaledDeltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.unscaledDeltaTime;
                float unscaledDeltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.unscaledDeltaTime;

                _cinemachineTargetPitch += inputManager.look.y * rotationSpeed * unscaledDeltaTimeMultiplier;
                rotationVelocity = inputManager.look.x * rotationSpeed * unscaledDeltaTimeMultiplier;

                // clamp our pitch rotation
                _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, bottomClamp, topClamp);

                // Update Cinemachine camera target pitch
                cinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

                // rotate the player left and right
                transform.Rotate(Vector3.up * rotationVelocity);
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
            if (isGrounded)
            {
                // stop our velocity dropping infinitely when grounded
                if (verticalVelocity < 0.0f)
                {
                    verticalVelocity = -2f;
                }
            }
            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (verticalVelocity < terminalVelocity)
            {
                verticalVelocity += gravity * Time.unscaledDeltaTime;
            }
            characterController.Move(new Vector3(0.0f, verticalVelocity, 0.0f) * Time.unscaledDeltaTime);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (isGrounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;



            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z), groundedRadius);
                        
        }

        public override void EnterState()
        {
            throw new NotImplementedException();
        }

        protected override void PhysicsUpdateThisState()
        {
            
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
