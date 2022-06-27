using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(InputManager))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementStateManager : AbstractClass.State
    {
        [Header("Player")]
        [Tooltip("Run speed of the character in m/s")]
        public float runWhileGroundedSpeed = 6.0f;

        public float runWhileAirborneSpeed = 6.0f;

        public float jumpBoostSpeed = 7.0f;

        [Tooltip("Acceleration and deceleration")]
        public float speedChangeRate = 10.0f;
        [Tooltip("Rotation speed of the character")]
        public float rotationSpeed = 1.0f;

        [Space(10)]
        [Tooltip("Dash speed of the character in m/s")]
        public float dashSpeed = 2f;
        [Tooltip("Dashing duration of each dash")]
        public float dashDuration = 0.25f;
        [Tooltip("Time required to pass before being able to dash again")]
        public float dashCooldown = 1f;

        public float dashChargeCooldown = 1f;

        public float dashDistanceWhileTimeSlowMultiflier = 1f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float jumpHeight = 1.2f;
        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float gravity = -15.0f;
        //terminalVelocity must be negative
        public float terminalVelocity = -53.0f;

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

        public Vector3 groundedBoxDimention = new Vector3(1, 1, 1);

        [Tooltip("What layers the character uses as ground")]
        public LayerMask groundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject cinemachineCameraTarget;
        [Tooltip("How far in degrees can you move the camera up")]
        public float topClamp = 90.0f;
        [Tooltip("How far in degrees can you move the camera down")]
        public float bottomClamp = -90.0f;

        public GameObject particleDash;

        //input direction
        public Vector3 inputDirection;

        // cinemachine
        private float _cinemachineTargetPitch;

        // player stats
        public float currentSpeed;
        public float targetSpeed;

        private float _rotationVelocity;
        public float verticalVelocity;

        public Vector3 airborneInertiaDirection = Vector3.zero;
        public Vector3 dashDirection = Vector3.zero;

        public float currentGravity = -15.0f;

        public bool isDashable = true;
        public bool isJumpable = true;
        public bool isDoubleJumpable = true;

        public bool isInDashState = false;
        public bool isInDashChargeCooldown = false;
        public int dashMaxCount = 2;
        public int dashCurrentCount = 2;

        private float previousStepOffset;

        // timeout deltaTime

        public PlayerInput playerInput;
        public CharacterController characterController;
        public InputManager inputManager;
        public PlayerSkillManager playerSkillManager;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool IsCurrentDeviceMouse => playerInput.currentControlScheme == "KeyboardMouse";

        //States logic
        private PlayerMovementIdleState _playerMovementIdleState;
        private PlayerRunState _playerRunState;
        private PlayerDashState _playerDashState;

        #region State Machine
        protected override void InitializeState()
        {
            _playerMovementIdleState = GetComponent<PlayerMovementIdleState>();
            _playerRunState = GetComponent<PlayerRunState>();
            _playerDashState = GetComponent<PlayerDashState>();

            _playerMovementIdleState.SetSuperState(this);
            _playerRunState.SetSuperState(this);
            _playerDashState.SetSuperState(this);

            SetSuperState(null);
            SetSubState(_playerMovementIdleState);
        }
        protected override void InitializeComponent()
        {
            characterController = GetComponent<CharacterController>();
            playerInput = GetComponent<PlayerInput>();
            inputManager = GetComponent<InputManager>();
            playerSkillManager = GetComponentInChildren<PlayerSkillManager>();
        }

        protected override void InitializeVariable()
        {
            EnableGravity();
            //register listener
            this.RegisterListener(EventID.onDodgePress, (param) => onDodgePress());
        }

        protected override void UpdateThisState()
        {
            CheckGrounded();
            HandleRunInput();
            ApplyGravity();
            Jump();            
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

        public override void SwitchToState(string p_stateType)
        {
            switch (p_stateType)
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
                default:
                    break;
            }
        }
        #endregion

        #region Unity functions
        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }
        private void Update()
        {
            UpdateAllState();
        }

        private void FixedUpdate()
        {
            PhysicsUpdateAllState();
        }
        private void LateUpdate()
        {
            Move();
            CheckDashChargeCooldown();
            Look();
        }
        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (isGrounded)
            {
                Gizmos.color = transparentGreen;
            }
            else
            {
                Gizmos.color = transparentRed;
            }

            Vector3 boxPosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawCube(boxPosition, groundedBoxDimention);
        }
        #endregion

        #region API
        public bool IsDashable()
        {
            return isDashable && dashCurrentCount > 0;
        }
        public void DisableJump()
        {
            isJumpable = false;
        }
        public void EnableJump()
        {
            isJumpable = true;
        }
        public void DisableDoubleJump()
        {
            isDoubleJumpable = false;
        }
        public void EnableDoubleJump()
        {
            isDoubleJumpable = true;
        }
        public void DisableGravity()
        {
            currentGravity = 0;
            verticalVelocity = 0;
        }

        public void EnableGravity()
        {
            currentGravity = gravity;
        }
        public void SetRunTargetSpeed()
        {
            targetSpeed = runWhileGroundedSpeed;
        }

        public void SetIdleTargetSpeed()
        {
            targetSpeed = 0;
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
        public void SetAirborneInertiaDirection()
        {
            airborneInertiaDirection = new Vector3(characterController.velocity.x, 0f, characterController.velocity.z).normalized;
        }
        public void SetDashDirection()
        {
            // TODO: dash to view point
            //dashDirection = transform.forward;
            dashDirection = inputDirection;
        }
        public void ResetDashDirection()
        {
            dashDirection = Vector3.zero;
        }

        public void StartCoroutineDashState()
        {
            dashCurrentCount -= 1;
            StartCoroutine(StartDashDuration());
        }

        #endregion

        #region Movement
        private void CheckGrounded()
        {
            // set sphere position, with offset
            Vector3 boxPosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
            isGrounded = Physics.CheckBox(boxPosition, groundedBoxDimention, Quaternion.identity, groundLayers, QueryTriggerInteraction.Ignore);
        }
        private void HandleRunInput()
        {
            inputDirection = transform.right * inputManager.move.x + transform.forward * inputManager.move.y;
            inputDirection.Normalize();
        }
        private void ApplyGravity()
        {
            if (isGrounded)
            {
                // stop our velocity dropping infinitely when grounded
                if (verticalVelocity < 0)
                {
                    verticalVelocity = 0;
                }
            }
            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (verticalVelocity > terminalVelocity)
            {
                verticalVelocity += currentGravity * Time.deltaTime;
            }
        }
        private void Jump()
        {
            if (isJumpable)
            {
                if (isGrounded && inputManager.IsButtonDownThisFrame("Jump"))
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    
                    //Audio
                    AudioInterface.PlayAudio("jump");
                }
                else if (isDoubleJumpable && inputManager.IsButtonDownThisFrame("Jump"))
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    DisableDoubleJump();

                    //Audio
                    AudioInterface.PlayAudio("jump");
                }
            }
        }        

        private void CheckDashChargeCooldown()
        {
            if (dashCurrentCount < dashMaxCount && !isInDashChargeCooldown && !isInDashState && isGrounded)
            {
                StartCoroutine(StartDashChargeCooldown());
            }
        }
        private void Move()
        {
            //smooth the speed change (momentum mechanic) 
            float inputMagnitude = inputManager.analogMovement ? inputManager.move.magnitude : 1f;
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed * inputMagnitude, Time.deltaTime * speedChangeRate);

            // TODO: delete old acceleration after testing
            //if (currentSpeed < targetSpeed)
            //{
            //    currentSpeed += acceleration * Time.deltaTime;
            //}
            //else if (currentSpeed > targetSpeed)
            //{
            //    currentSpeed -= acceleration * Time.deltaTime;
            //}

            // move the player
            //characterController.Move(inputDirection.normalized * currentSpeed * Time.deltaTime + new Vector3(airborneDirection.x * airborneDefaultSpeed, verticalVelocity, airborneDirection.z * airborneDefaultSpeed) * Time.deltaTime + new Vector3(dashDirection.x * dashSpeed, 0f, dashDirection.z * dashSpeed) * Time.unscaledDeltaTime);
            // TODO: dash to view point
            if (isGrounded)
            {
                characterController.Move(
                    inputDirection * currentSpeed * Time.deltaTime
                    + new Vector3(0f, verticalVelocity, 0f) * Time.deltaTime
                    + new Vector3(dashDirection.x * dashSpeed, 0f, dashDirection.z * dashSpeed) * Time.unscaledDeltaTime);
                // TODO: grounded input have non uniform speed
            }
            else
            {
                characterController.Move(
                    airborneInertiaDirection * currentSpeed * Time.deltaTime
                    + new Vector3(0f, verticalVelocity, 0f) * Time.deltaTime
                    + new Vector3(dashDirection.x * dashSpeed, 0f, dashDirection.z * dashSpeed) * Time.unscaledDeltaTime);
                // TODO: airborne input have a small effects not none
                //+ airborneInputDirection * runWhileAirborneSpeed * Time.deltaTime
            }
        }
        private void Look()
        {
            // TODO: perform check like this to optimize the update loop
            // if there is an input
            if (inputManager.look.sqrMagnitude >= _threshold)
            {
                //Don't multiply mouse input by Time.unscaledDeltaTime
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetPitch += inputManager.look.y * rotationSpeed * deltaTimeMultiplier;
                _rotationVelocity = inputManager.look.x * rotationSpeed * deltaTimeMultiplier;

                // clamp our pitch rotation
                _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, bottomClamp, topClamp);

                // Update Cinemachine camera target pitch
                cinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

                // rotate the player left and right
                transform.Rotate(Vector3.up * _rotationVelocity);
            }
        }

        #endregion

        #region Coroutine
        // TODO: unused jump cooldown behavior
        //IEnumerator StartJumpCooldown()
        //{
        //    isJumpable = false;
        //    yield return new WaitForSeconds(jumpCooldown);
        //    isJumpable = true;
        //}
        //public void StartCoroutineStartJumpCooldown()
        //{
        //    StartCoroutine(StartJumpCooldown());
        //}
        private IEnumerator StartDashCooldown()
        {
            yield return new WaitForSeconds(dashCooldown);
            isDashable = true;
        }
        private IEnumerator StartDashChargeCooldown()
        {
            isInDashChargeCooldown = true;
            yield return new WaitForSeconds(dashChargeCooldown);
            isInDashChargeCooldown = false;
            dashCurrentCount += 1;
        }

        private IEnumerator StartDashDuration()
        {
            isDashable = false;
            isInDashState = true;
            if (particleDash)
            {
                if (!particleDash.activeSelf)
                {
                    particleDash.SetActive(true);
                }
            }
            AudioInterface.PlayAudio("dash");
            if (playerSkillManager.gameIsSlowDown)
            {
                yield return new WaitForSeconds(dashDuration * Time.timeScale * dashDistanceWhileTimeSlowMultiflier);
            }
            else
            {
                yield return new WaitForSeconds(dashDuration);
            }
            if (particleDash)
            {
                if (particleDash.activeSelf)
                {
                    particleDash.SetActive(false);
                }
            }
            isInDashState = false;
            ResetDashDirection();
            StartCoroutine(StartDashCooldown());
        }
        #endregion

        #region Helper functions
        private void AddJumpBoostSpeed()
        {
            targetSpeed += jumpBoostSpeed;
        }
        private void onDodgePress()
        {
            Debug.Log("event successfully register!");
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
        #endregion

    }
}
