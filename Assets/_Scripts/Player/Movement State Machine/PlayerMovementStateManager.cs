using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(InputManager))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerSkillManager))]

    [RequireComponent(typeof(PlayerMovementIdleState))]
    [RequireComponent(typeof(PlayerRunState))]
    [RequireComponent(typeof(PlayerDashState))]
    [RequireComponent(typeof(PlayerSlideState))]
    [RequireComponent(typeof(PlayerCrouchState))]
    [RequireComponent(typeof(PlayerWallRunState))]
    public class PlayerMovementStateManager : AbstractClass.State
    {
        [Header("Player")]
        [Tooltip("Run speed of the character in m/s")]
        public float runSpeed = 6.0f;

        [Space]
        [Tooltip("Speed of the character in m/s")]
        public float slideThresholdSpeed = 6.0f;
        public float slideSpeed = 6.0f;
        public float slideDuration = 1f;
        public float slideJumpSpeed = 20f;
        public float slideGravity = -1000f;

        [Space]
        [Tooltip("Crouch speed of the character in m/s")]
        public float crouchSpeed = 6.0f;

        [Space]
        [Tooltip("Acceleration and deceleration")]
        public float speedChangeRate = 10.0f;
        [Tooltip("Rotation speed of the character")]
        public float rotationSpeed = 1.0f;

        public float airborneSteeringRate = 1f;

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

        [Space(10)]
        [Tooltip("Crouch height of character")]
        public float crouchHeight = 1.6f;
        public Vector3 crouchCenter = new Vector3(0, 0.93f, 0);
        public float timeToCrouch = 0.25f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool isGrounded = true;
        [Tooltip("Useful for rough ground")]
        public float groundedOffset = -0.14f;

        public Vector3 groundedBoxDimention = new Vector3(1, 1, 1);

        [Tooltip("What layers the character uses as ground")]
        public LayerMask groundLayers;

        [Header("Player Roofed")]
        public bool isRoofed = true;
        public float roofedOffset = 1.94f;
        public float roofedDistance = 0.22f;
        public LayerMask roofedLayers;

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
        public Vector3 slideDirection = Vector3.zero;
        public Vector3 dashDirection = Vector3.zero;

        public float currentGravity = -15.0f;

        public bool isJumpable = true;
        public bool isDashable = true;
        public bool isDoubleJumpable = true;

        public bool isInDashState = false;
        public bool isInDashChargeCooldown = false;
        public int dashMaxCount = 2;
        public int dashCurrentCount = 2;

        private float originalStepOffset;
        private float originalCharacterHeight;
        private Vector3 originalCharacterCenter;
        private float originalCamHolderHeight;

        private IEnumerator _slideCoroutine;
        private IEnumerator _crouchDownCoroutine;
        private IEnumerator _standUpCoroutine;

        public PlayerInput playerInput;
        public CharacterController characterController;
        public InputManager inputManager;
        public PlayerSkillManager playerSkillManager;
        private GameObject _mainCamera;
        private CapsuleCollider _capsuleCollider;

        private const float _threshold = 0.01f;

        private bool IsCurrentDeviceMouse => playerInput.currentControlScheme == "KeyboardMouse";

        //States logic
        private PlayerMovementIdleState _playerMovementIdleState;
        private PlayerRunState _playerRunState;
        private PlayerDashState _playerDashState;
        private PlayerSlideState _playerSlideState;
        private PlayerCrouchState _playerCrouchState;
        private PlayerWallRunState _playerWallRunState;

        #region State Machine
        protected override void InitializeState()
        {
            _playerMovementIdleState = GetComponent<PlayerMovementIdleState>();
            _playerRunState = GetComponent<PlayerRunState>();
            _playerDashState = GetComponent<PlayerDashState>();
            _playerSlideState = GetComponent<PlayerSlideState>();
            _playerCrouchState = GetComponent<PlayerCrouchState>();
            _playerWallRunState = GetComponent<PlayerWallRunState>();

            _playerMovementIdleState.SetSuperState(this);
            _playerRunState.SetSuperState(this);
            _playerDashState.SetSuperState(this);
            _playerSlideState.SetSuperState(this);
            _playerCrouchState.SetSuperState(this);
            _playerWallRunState.SetSuperState(this);

            SetSuperState(null);
            SetSubState(_playerMovementIdleState);
        }
        protected override void InitializeComponent()
        {
            characterController = GetComponent<CharacterController>();
            playerInput = GetComponent<PlayerInput>();
            inputManager = GetComponent<InputManager>();
            playerSkillManager = GetComponentInChildren<PlayerSkillManager>();
            _capsuleCollider = GetComponent<CapsuleCollider>();
        }

        protected override void InitializeVariable()
        {
            EnableRunGravity();
            //register listener
            this.RegisterListener(EventID.onDodgePress, (param) => onDodgePress());

            //set original controller value
            originalCharacterHeight = characterController.height;
            originalCharacterCenter = characterController.center;
            originalCamHolderHeight = cinemachineCameraTarget.transform.localPosition.y;
        }
        private void onDodgePress()
        {
            Debug.Log("event successfully register!");
        }
        private void onDodgePress()
        {
            Debug.Log("event successfully register!");
        }

        protected override void UpdateThisState()
        {
            CheckGrounded();
            CheckRoofed();
            HandleRunInput();
            ApplyGravity();
            Jump();
            HandleSpeed();
            CheckDashChargeCooldown();
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
                case "Slide":
                    SetSubState(_playerSlideState);
                    break;
                case "Crouch":
                    SetSubState(_playerCrouchState);
                    break;
                case "WallRun":
                    SetSubState(_playerWallRunState);
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
            Gizmos.DrawCube(boxPosition, groundedBoxDimention * 2);
        }
        #endregion

        #region API   
        public void SetSlideJumpTargetSpeed()
        {
            targetSpeed = slideJumpSpeed;
        }
        public bool IsSlideable()
        {
            return currentSpeed > slideThresholdSpeed;
        }

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

        public void EnableRunGravity()
        {
            currentGravity = gravity;
        }
        public void EnableSlideGravity()
        {
            currentGravity = slideGravity;
        }
        public void DisableSlideGravity()
        {
            EnableRunGravity();
        }
        public void SetRunTargetSpeed()
        {
            targetSpeed = runSpeed;
        }

        public void SetIdleTargetSpeed()
        {
            targetSpeed = 0;
        }
        public void SetSlideTargetSpeed()
        {
            targetSpeed = slideSpeed;
        }
        public void SetCrouchTargetSpeed()
        {
            targetSpeed = crouchSpeed;
        }
        public void StopSpeedChange()
        {
            targetSpeed = currentSpeed;
        }
        public void DisableStepOffset()
        {
            originalStepOffset = characterController.stepOffset;
            characterController.stepOffset = 0;
        }
        public void EnableStepOffset()
        {
            characterController.stepOffset = originalStepOffset;
        }
        public void SetAirborneInertiaDirection()
        {
            airborneInertiaDirection = new Vector3(characterController.velocity.x, 0f, characterController.velocity.z).normalized;
        }
        public void SetAirborneInertiaDirectionWhileDoubleJump()
        {
            if (moveDirection != Vector3.zero)
            {
                airborneInertiaDirection = moveDirection;
            }
        }
        public void SetAirborneInertiaDirectionWhileDash()
        {
            airborneInertiaDirection = dashDirection;
        }
        public void SetSlideDirection()
        {
            slideDirection = inputDirection;
        }
        public void SetDashDirection()
        {
            // TODO: dash to view point
            Vector3 rot = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f) * Vector3.forward;
            dashDirection = transform.right * rot.x + transform.up * rot.y + transform.forward * rot.z;
        }
        public void ResetDashDirection()
        {
            dashDirection = Vector3.zero;
        }

        public void ResetMoveDirection()
        {
            inputDirection = Vector3.zero;
        }

        public void StartCoroutineDashState()
        {
            dashCurrentCount -= 1;
            StartCoroutine(StartDashDuration());
        }
        public void StartCoroutineSlideState()
        {
            _slideCoroutine = StartSlideDuration();
            StartCoroutine(_slideCoroutine);
        }
        public void StopCoroutineSlideState()
        {
            StopCoroutine(_slideCoroutine);
        }

        public void MoveWhileGrounded()
        {
            // move the player            
            // TODO: momentum conservation
            characterController.Move(
                    inputDirection * currentSpeed * Time.deltaTime
                    + Vector3.up * verticalVelocity * Time.deltaTime);
        }
        public void MoveWhileAirborne()
        {
            HandleAirborneSteering();

            characterController.Move(
                    airborneInertiaDirection * currentSpeed * Time.deltaTime
                    + Vector3.up * verticalVelocity * Time.deltaTime);

        }
        private void HandleAirborneSteering()
        {
            if (inputDirection != Vector3.zero)
            {
                airborneInertiaDirection = Vector3.Lerp(airborneInertiaDirection, inputDirection, Time.deltaTime * airborneSteeringRate);
                airborneInertiaDirection.Normalize();
            }
        }
        public void MoveWhileSlide()
        {
            characterController.Move(
                    slideDirection * currentSpeed * Time.deltaTime
                    + Vector3.up * verticalVelocity * Time.deltaTime);
        }
        public void MoveWhileDash()
        {
            characterController.Move(dashDirection * dashSpeed * Time.unscaledDeltaTime);
        }
        public void MoveWhileWallRun()
        {

        }

        public void StartCoroutineCrouchDown()
        {
            _crouchDownCoroutine = CrouchDown();
            StopCoroutine(_standUpCoroutine);
            StartCoroutine(_crouchDownCoroutine);
        }
        public void StarCoroutineStandUp()
        {
            _standUpCoroutine = StandUp();
            StopCoroutine(_crouchDownCoroutine);
            StartCoroutine(_standUpCoroutine);
        }

        #endregion

        #region Movement
        private void CheckGrounded()
        {
            // set sphere position, with offset
            Vector3 boxPosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
            isGrounded = Physics.CheckBox(boxPosition, groundedBoxDimention, Quaternion.identity, groundLayers, QueryTriggerInteraction.Ignore);
        }
        private void CheckRoofed()
        {
            // set sphere position, with offset
            Vector3 rayPosition = new Vector3(transform.position.x, transform.position.y + roofedOffset, transform.position.z);
            isRoofed = Physics.Raycast(rayPosition, Vector3.up, roofedDistance, roofedLayers, QueryTriggerInteraction.Ignore);
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
                if (verticalVelocity < -2f)
                {
                    verticalVelocity = -2f;
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
                    SetAirborneInertiaDirectionWhileDoubleJump();

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
        private void HandleSpeed()
        {
            //smooth the speed change (momentum mechanic) 
            float inputMagnitude = inputManager.analogMovement ? inputManager.move.magnitude : 1f;
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed * inputMagnitude, Time.deltaTime * speedChangeRate);
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
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
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
        private IEnumerator CrouchDown()
        {
            float timeElapsed = 0;
            while (timeElapsed < timeToCrouch)
            {
                characterController.height = Mathf.Lerp(characterController.height, crouchHeight, timeElapsed / timeToCrouch);
                characterController.center = Vector3.Lerp(characterController.center, crouchCenter, timeElapsed / timeToCrouch);

                _capsuleCollider.height = characterController.height;
                _capsuleCollider.center = characterController.center;

                cinemachineCameraTarget.transform.localPosition = new Vector3(cinemachineCameraTarget.transform.localPosition.x, originalCamHolderHeight - (originalCharacterHeight - characterController.height), cinemachineCameraTarget.transform.localPosition.z);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
        private IEnumerator StandUp()
        {
            float timeElapsed = 0;
            while (timeElapsed < timeToCrouch)
            {
                characterController.height = Mathf.Lerp(characterController.height, originalCharacterHeight, timeElapsed / timeToCrouch);
                characterController.center = Vector3.Lerp(characterController.center, originalCharacterCenter, timeElapsed / timeToCrouch);

                _capsuleCollider.height = characterController.height;
                _capsuleCollider.center = characterController.center;

                cinemachineCameraTarget.transform.localPosition = new Vector3(cinemachineCameraTarget.transform.localPosition.x, originalCamHolderHeight - (originalCharacterHeight - characterController.height), cinemachineCameraTarget.transform.localPosition.z);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
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

        private IEnumerator StartSlideDuration()
        {
            yield return new WaitForSeconds(slideDuration);
            SwitchToState("Crouch");
        }
        #endregion

    }
}
