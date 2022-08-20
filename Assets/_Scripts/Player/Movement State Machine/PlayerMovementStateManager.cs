using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(InputManager))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerSkillStateManager))]

    [RequireComponent(typeof(PlayerIdleMovementState))]
    [RequireComponent(typeof(PlayerRunState))]
    [RequireComponent(typeof(PlayerDashState))]
    [RequireComponent(typeof(PlayerSlideState))]
    [RequireComponent(typeof(PlayerCrouchState))]
    [RequireComponent(typeof(PlayerWallRunState))]
    [RequireComponent(typeof(PlayerLedgeGrabState))]
    public class PlayerMovementStateManager : AbstractClass.State
    {
        [Header("Run")]
        [Tooltip("Run speed of the character in m/s")]
        [SerializeField] private float runSpeed = 6.0f;

        [Space]
        [Header("Slide")]
        [Tooltip("Speed of the character in m/s")]
        [SerializeField] private float slideThresholdSpeed = 6.0f;
        [SerializeField] private float slideSpeed = 6.0f;
        [SerializeField] private float slideDuration = 1f;
        [SerializeField] private float slideJumpSpeed = 20f;
        [SerializeField] private float slideGravity = -1000f;

        [Space]
        [Header("Crouch")]
        [Tooltip("Crouch speed of the character in m/s")]
        [SerializeField] private float crouchSpeed = 6.0f;

        [Space]
        [Header("Dash")]
        [Tooltip("Dash speed of the character in m/s")]
        [SerializeField] private float dashSpeed = 2f;
        [Tooltip("Dashing duration of each dash")]
        [SerializeField] private float dashDuration = 0.25f;
        [Tooltip("Time required to pass before being able to dash again")]
        [SerializeField] private float dashCooldown = 1f;

        [SerializeField] private int dashMaxChargeCount = 2;

        [SerializeField] private float dashChargeCooldown = 1f;

        [SerializeField] private string playerPhaseLayerName = "PlayerPhase";

        [SerializeField] private float dashFOVMultiplier = 1f;

        [SerializeField] private float dashFOVRevertDuration = 1f;

        [Space]
        [Header("Ledge grab")]
        [SerializeField] private float ledgeGrabDuration = 2f;

        [SerializeField] private float ledgeGrabSpeed = 2f;

        [SerializeField][Range(0, 90)] private float ledgeGrabClimbAngle = 45;

        [SerializeField] private Vector3 ledgeGrabBoxSize = Vector3.one;

        [SerializeField] private Vector3 ledgeGrabBoxCenterOffset = Vector3.zero;

        [SerializeField] private LayerMask ledgeGrabLayers;

        [Space]
        [Header("Change rate")]
        [Tooltip("Acceleration and deceleration")]
        [SerializeField] private float speedChangeRate = 10.0f;
        [Tooltip("Rotation speed of the character")]
        [SerializeField] private float rotationSpeed = 1.0f;

        [SerializeField] private float airborneSteeringRate = 1f;

        [SerializeField] private float airborneSpeedChangeRate = 1f;

        [Space]
        [Header("Jump")]
        [Tooltip("The height the player can jump")]
        [SerializeField] private float jumpHeight = 1.2f;

        [SerializeField] private float coyoteTime = 0.2f;
        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        [SerializeField] private float gravity = -15.0f;
        [Tooltip("Terminal Velocity must be negative")]
        [SerializeField] private float terminalVelocity = -53.0f;

        // Unused behaviors
        //[Space]
        //[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        //public float jumpCooldown = 0.1f;
        //[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        //public float airborneCooldown = 0.15f;

        [Space]
        [Header("Crouch")]
        [Tooltip("Crouch height of character")]
        [SerializeField] private float crouchHeight = 1.6f;
        [SerializeField] private Vector3 crouchCenter = new Vector3(0, 0.93f, 0);
        [SerializeField] private float timeToCrouch = 0.25f;

        [Space]
        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool isGrounded = true;
        [Tooltip("Useful for rough ground")]
        [SerializeField] private float groundedOffset = -0.14f;
        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float groundedRadius = 0.5f;
        [Tooltip("What layers the character uses as ground")]
        [SerializeField] private LayerMask groundLayers;

        [Space]
        [Header("Player Roofed")]
        public bool isRoofed = true;
        [SerializeField] private float roofedOffset = 1.94f;
        [SerializeField] private float roofedDistance = 0.22f;
        [SerializeField] private LayerMask roofedLayers;

        [Space]
        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        [SerializeField] private GameObject cinemachineCameraTarget;
        [Tooltip("How far in degrees can you move the camera up")]
        [SerializeField] private float topClamp = 90.0f;
        [Tooltip("How far in degrees can you move the camera down")]

        [SerializeField] private float bottomClamp = -90.0f;

        [Space]
        [Header("Particle")]
        public GameObject particleDash;

        [Space]
        //input direction
        public Vector3 inputDirection;

        // cinemachine
        private float _cinemachineTargetPitch;

        // player stats
        private float currentSpeed;
        private float targetSpeed;

        private float _rotationVelocity;
        private float verticalVelocity;

        private Vector3 airborneInertiaDirection = Vector3.zero;
        private Vector3 slideDirection = Vector3.zero;
        private Vector3 dashDirection = Vector3.zero;
        private Vector3 ledgeGrabDirection = Vector3.zero;

        private float currentGravity = -15.0f;

        private bool isCoyoteTime = false;
        private bool isDashable = true;
        private bool isDoubleJumpable = true;

        public bool isInDashState = false;
        private bool isInDashChargeCooldown = false;
        private int dashCurrentCount = 2;

        private float _originalStepOffset;
        private float _originalCharacterHeight;
        private Vector3 _originalCharacterCenter;
        private float _originalCamHolderHeight;
        private int _originalPlayerLayer;
        private float _originalFOV;
        private float _originalDashChargeCooldown;

        private IEnumerator _slideCoroutine;
        private IEnumerator _crouchDownCoroutine;
        private IEnumerator _standUpCoroutine;
        private IEnumerator _changeFOVWhileDashCoroutine;
        private IEnumerator _revertFOVAfterDashCoroutine;
        private IEnumerator _changeFOVWhileSlideCoroutine;
        private IEnumerator _revertFOVAfterSlideCoroutine;
        private IEnumerator _coyoteTimeCountDownCoroutine;
        private IEnumerator _ledgeGrabCoroutine;

        private PlayerInput _playerInput;
        private CharacterController _characterController;
        public InputManager inputManager;
        private PlayerSkillStateManager _playerSkillManager;
        private CapsuleCollider _characterCapsuleCollider;
        private PlayerActionStateManager _playerActionStateManager;
        private CinemachineVirtualCamera _cinemachineVirtualCamera;
        public Animator animator;

        private const float _threshold = 0.01f;

        private bool IsCurrentDeviceMouse => _playerInput.currentControlScheme == "KeyboardMouse";

        //States logic
        private PlayerIdleMovementState _playerIdleMovementState;
        private PlayerRunState _playerRunState;
        private PlayerDashState _playerDashState;
        private PlayerSlideState _playerSlideState;
        private PlayerCrouchState _playerCrouchState;
        private PlayerWallRunState _playerWallRunState;
        private PlayerLedgeGrabState _playerLedgeGrabState;

        #region State Machine
        protected override void InitializeState()
        {
            _playerIdleMovementState = GetComponent<PlayerIdleMovementState>();
            _playerRunState = GetComponent<PlayerRunState>();
            _playerDashState = GetComponent<PlayerDashState>();
            _playerSlideState = GetComponent<PlayerSlideState>();
            _playerCrouchState = GetComponent<PlayerCrouchState>();
            _playerWallRunState = GetComponent<PlayerWallRunState>();
            _playerLedgeGrabState = GetComponent<PlayerLedgeGrabState>();

            _playerIdleMovementState.SetSuperState(this);
            _playerRunState.SetSuperState(this);
            _playerDashState.SetSuperState(this);
            _playerSlideState.SetSuperState(this);
            _playerCrouchState.SetSuperState(this);
            _playerWallRunState.SetSuperState(this);
            _playerLedgeGrabState.SetSuperState(this);

            SetSuperState(null);
            SetSubState(_playerIdleMovementState);
        }
        protected override void InitializeComponent()
        {
            _characterController = GetComponent<CharacterController>();
            _playerInput = GetComponent<PlayerInput>();
            inputManager = GetComponent<InputManager>();
            _playerSkillManager = GetComponentInChildren<PlayerSkillStateManager>();
            _characterCapsuleCollider = GetComponent<CapsuleCollider>();
            _playerActionStateManager = GetComponent<PlayerActionStateManager>();
            animator = GetComponentInChildren<Animator>();
        }

        protected override void InitializeVariable()
        {
            EnableRunGravity();

            //set original controller value
            _originalCharacterHeight = _characterController.height;
            _originalCharacterCenter = _characterController.center;
            _originalCamHolderHeight = cinemachineCameraTarget.transform.localPosition.y;

            _cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            _originalFOV = _cinemachineVirtualCamera.m_Lens.FieldOfView;

            _originalPlayerLayer = gameObject.layer;
            _originalDashChargeCooldown = dashChargeCooldown;

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
                    SetSubState(_playerIdleMovementState);
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
                case "LedgeGrab":
                    SetSubState(_playerLedgeGrabState);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Unity functions

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

            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(spherePosition, groundedRadius);

            if (isRoofed)
            {
                Gizmos.color = transparentGreen;
            }
            else
            {
                Gizmos.color = transparentRed;
            }

            try
            {
                Vector3 rayPosition = new Vector3(transform.position.x, transform.position.y + _characterController.height + roofedOffset, transform.position.z);
                Gizmos.DrawRay(rayPosition, Vector3.up * roofedDistance);
            }
            catch
            {

            }

            Gizmos.color = transparentGreen;
            Vector3 boxPosition = transform.position + transform.rotation * ledgeGrabBoxCenterOffset;
            Gizmos.DrawWireCube(boxPosition, ledgeGrabBoxSize);
        }
        #endregion

        #region API
        public void EnableUnlimitedDash()
        {
            dashChargeCooldown = 0;
        }
        public void DisableUnlimitedDash()
        {
            dashChargeCooldown = _originalDashChargeCooldown;
        }
        public void EnablePhaseThroughEnemy()
        {
            gameObject.layer = LayerMask.NameToLayer(playerPhaseLayerName);
        }
        public void DisablePhaseThroughEnemy()
        {
            gameObject.layer = _originalPlayerLayer;
        }
        public void EnableAttackHitbox()
        {
            _playerActionStateManager.EnableAttackHitbox();
        }
        public void DisableAttackHitbox()
        {
            _playerActionStateManager.DisableAttackHitbox();
        }
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
            _originalStepOffset = _characterController.stepOffset;
            _characterController.stepOffset = 0;
        }
        public void EnableStepOffset()
        {
            _characterController.stepOffset = _originalStepOffset;
        }
        public void SetAirborneInertiaDirection()
        {
            airborneInertiaDirection = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.z).normalized;
        }
        //redundant behavior
        //public void SetAirborneInertiaDirectionWhileDoubleJump()
        //{
        //    if (inputDirection != Vector3.zero)
        //    {
        //        airborneInertiaDirection = inputDirection;
        //    }
        //}
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
            Vector3 localDirection = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f) * Vector3.forward;
            dashDirection = transform.right * localDirection.x + transform.up * localDirection.y + transform.forward * localDirection.z;
        }
        public void ResetDashDirection()
        {
            dashDirection = Vector3.zero;
        }

        public void ResetMoveDirection()
        {
            inputDirection = Vector3.zero;
        }

        public void SetLedgeGrabDirection()
        {
            Vector3 localDirection = Quaternion.Euler(-ledgeGrabClimbAngle, 0.0f, 0.0f) * Vector3.forward;
            ledgeGrabDirection = transform.right * localDirection.x + transform.up * localDirection.y + transform.forward * localDirection.z;
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
            _characterController.Move(
                    inputDirection * currentSpeed * Time.unscaledDeltaTime
                    + Vector3.up * verticalVelocity * Time.unscaledDeltaTime);
        }
        public void MoveWhileAirborne()
        {
            HandleAirborneSteering();

            _characterController.Move(
                    airborneInertiaDirection * currentSpeed * Time.unscaledDeltaTime
                    + Vector3.up * verticalVelocity * Time.unscaledDeltaTime);

        }
        private void HandleAirborneSteering()
        {
            if (inputDirection != Vector3.zero)
            {
                if (inputManager.move.y >= 0)
                {
                    airborneInertiaDirection = Vector3.RotateTowards(airborneInertiaDirection, inputDirection, airborneSteeringRate * Time.unscaledDeltaTime, 0.0f);
                }
                else
                {
                    Vector2 steeringDirection = transform.right * inputManager.move.x;
                    steeringDirection.Normalize();
                    airborneInertiaDirection = Vector3.RotateTowards(airborneInertiaDirection, steeringDirection, airborneSteeringRate * Time.unscaledDeltaTime, 0.0f);

                    currentSpeed = Mathf.MoveTowards(currentSpeed, 0, airborneSpeedChangeRate * Time.unscaledDeltaTime);
                    StopSpeedChange();
                }
            }
        }
        public void MoveWhileSlide()
        {
            _characterController.Move(
                    slideDirection * currentSpeed * Time.unscaledDeltaTime
                    + Vector3.up * verticalVelocity * Time.unscaledDeltaTime);
        }
        public void MoveWhileDash()
        {
            _characterController.Move(dashDirection * dashSpeed * Time.unscaledDeltaTime);
        }
        public void MoveWhileWallRun()
        {

        }

        public void MoveWhileLedgeGrab()
        {
            _characterController.Move(
                ledgeGrabDirection * ledgeGrabSpeed * Time.unscaledDeltaTime);
        }

        public void StartCoroutineCrouchDown()
        {
            try
            {
                StopCoroutine(_standUpCoroutine);
            }
            catch
            {

            }

            _crouchDownCoroutine = CrouchDown();
            StartCoroutine(_crouchDownCoroutine);
        }
        public void StarCoroutineStandUp()
        {
            try
            {
                StopCoroutine(_crouchDownCoroutine);
            }
            catch
            {

            }

            _standUpCoroutine = StandUp();
            StartCoroutine(_standUpCoroutine);
        }
        public void StartCoroutineChangeFOVWhileDash()
        {
            try
            {
                StopCoroutine(_revertFOVAfterDashCoroutine);
            }
            catch
            {

            }

            _changeFOVWhileDashCoroutine = ChangeFOVWhileDash();
            StartCoroutine(_changeFOVWhileDashCoroutine);
        }

        public void StarCoroutineRevertFOVAfterDash()
        {
            try
            {
                StopCoroutine(_changeFOVWhileDashCoroutine);
            }
            catch
            {

            }

            _revertFOVAfterDashCoroutine = RevertFOVAfterDash();
            StartCoroutine(_revertFOVAfterDashCoroutine);
        }
        public void StartCoroutineChangeFOVWhileSlide()
        {
            try
            {
                StopCoroutine(_revertFOVAfterSlideCoroutine);
            }
            catch
            {

            }

            _changeFOVWhileSlideCoroutine = ChangeFOVWhileSlide();
            StartCoroutine(_changeFOVWhileSlideCoroutine);
        }

        public void StarCoroutineRevertFOVAfterSlide()
        {
            try
            {
                StopCoroutine(_changeFOVWhileSlideCoroutine);
            }
            catch
            {

            }

            _revertFOVAfterSlideCoroutine = RevertFOVAfterSlide();
            StartCoroutine(_revertFOVAfterSlideCoroutine);
        }

        public void StartCoroutineLedgeGrabState()
        {
            StopCoroutineLedgeGrabState();
            _ledgeGrabCoroutine = StartLedgeGrabDuration();
            StartCoroutine(_ledgeGrabCoroutine);
        }

        public void StopCoroutineLedgeGrabState()
        {
            try
            {
                StopCoroutine(_ledgeGrabCoroutine);
            }
            catch
            {

            }
        }

        public bool CheckLedgeGrab()
        {
            Vector3 boxPosition = transform.position + transform.rotation * ledgeGrabBoxCenterOffset;
            return Physics.CheckBox(boxPosition, ledgeGrabBoxSize / 2, transform.rotation, ledgeGrabLayers);
            //bool result= Physics.CheckBox(boxPosition, ledgeGrabBoxSize / 2, transform.rotation, ledgeGrabLayers);
            //Debug.Log(result);
            //return result;
        }

        #endregion

        #region Movement
        private void CheckGrounded()
        {
            bool lastFrameIsGrounded = isGrounded;

            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
            isGrounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);

            if (isGrounded != lastFrameIsGrounded && !isGrounded && _characterController.velocity.y <= 0)
            {
                isCoyoteTime = true;

                _coyoteTimeCountDownCoroutine = StartCoyoteTimeCountDown();
                StartCoroutine(_coyoteTimeCountDownCoroutine);
            }
        }
        private IEnumerator StartCoyoteTimeCountDown()
        {
            yield return new WaitForSecondsRealtime(coyoteTime);
            isCoyoteTime = false;
        }
        private void CheckRoofed()
        {
            // set sphere position, with offset
            Vector3 rayPosition = new Vector3(transform.position.x, transform.position.y + _characterController.height + roofedOffset, transform.position.z);
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
            //ceiling bounce
            if (isRoofed && !isGrounded)
            {
                verticalVelocity = -verticalVelocity;
            }
        }
        private void Jump()
        {
            if (!isRoofed)
            {
                if (isGrounded && inputManager.jump)
                {
                    DisableSlideGravity();
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * currentGravity);

                    //Audio
                    this.PostEvent(EventID.onPlaySound, AudioID.jump);

                    animator.SetTrigger("isJump");
                }
                else if (isCoyoteTime && inputManager.IsButtonDownThisFrame("Jump"))
                {
                    // slide coyote jump will be a lot higher than normal jump
                    DisableSlideGravity();
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * currentGravity);

                    //Audio
                    this.PostEvent(EventID.onPlaySound, AudioID.jump);

                    animator.SetTrigger("isJump");

                    try
                    {
                        StopCoroutine(_coyoteTimeCountDownCoroutine);
                    }
                    catch
                    {

                    }
                    isCoyoteTime = false;
                }
                else if (isDoubleJumpable && inputManager.IsButtonDownThisFrame("Jump"))
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * currentGravity);
                    DisableDoubleJump();
                    //SetAirborneInertiaDirectionWhileDoubleJump();

                    //Audio
                    this.PostEvent(EventID.onPlaySound, AudioID.secondJump);

                    animator.SetTrigger("isJump");
                }
            }
        }

        private void CheckDashChargeCooldown()
        {
            if (dashCurrentCount < dashMaxChargeCount && !isInDashChargeCooldown && !isInDashState && isGrounded)
            {
                StartCoroutine(StartDashChargeCooldown());
            }
        }
        private void HandleSpeed()
        {
            // TODO: analog handling is wrong or rather, by the use of vector normalizing, the analog input is not handled correctly 
            //smooth the speed change (momentum mechanic) 
            float inputMagnitude = inputManager.analogMovement ? inputManager.move.magnitude : 1f;
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed * inputMagnitude, speedChangeRate * Time.unscaledDeltaTime);
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

        public void RotateWindParticle()
        {
            particleDash.transform.localRotation = Quaternion.Euler(particleDash.transform.localRotation.x,
            Vector3.SignedAngle(new Vector3(-inputManager.move.x, 0f, inputManager.move.y),
            _cinemachineVirtualCamera.transform.right, Vector3.up) + 90f, particleDash.transform.localRotation.z);
        }

        public void ResetWindParticleRotation()
        {
            particleDash.transform.localRotation = Quaternion.Euler(180, 0, 0);
        }

        #endregion

        #region Coroutine
        // TODO: unused jump cooldown behavior
        //IEnumerator StartJumpCooldown()
        //{
        //    isCoyoteTime = false;
        //    yield return new WaitForSeconds(jumpCooldown);
        //    isCoyoteTime = true;
        //}
        //public void StartCoroutineStartJumpCooldown()
        //{
        //    StartCoroutine(StartJumpCooldown());
        //}
        private IEnumerator CrouchDown()
        {
            while (_characterController.height > crouchHeight)
            {
                _characterController.height = Universal.Smoothing.LinearSmoothFixedTime(_characterController.height, _originalCharacterHeight, crouchHeight, Time.unscaledDeltaTime, timeToCrouch);
                _characterController.center = Vector3.MoveTowards(_characterController.center, crouchCenter, Vector3.Distance(_originalCharacterCenter, crouchCenter) / timeToCrouch * Time.unscaledDeltaTime);

                _characterCapsuleCollider.height = _characterController.height;
                _characterCapsuleCollider.center = _characterController.center;

                cinemachineCameraTarget.transform.localPosition = new Vector3(cinemachineCameraTarget.transform.localPosition.x, _originalCamHolderHeight - (_originalCharacterHeight - _characterController.height), cinemachineCameraTarget.transform.localPosition.z);
                yield return null;
            }
        }
        private IEnumerator StandUp()
        {
            while (_characterController.height < _originalCharacterHeight)
            {
                _characterController.height = Universal.Smoothing.LinearSmoothFixedTime(_characterController.height, crouchHeight, _originalCharacterHeight, Time.unscaledDeltaTime, timeToCrouch);
                _characterController.center = Vector3.MoveTowards(_characterController.center, _originalCharacterCenter, Vector3.Distance(_originalCharacterCenter, crouchCenter) / timeToCrouch * Time.unscaledDeltaTime);

                _characterCapsuleCollider.height = _characterController.height;
                _characterCapsuleCollider.center = _characterController.center;

                cinemachineCameraTarget.transform.localPosition = new Vector3(cinemachineCameraTarget.transform.localPosition.x, _originalCamHolderHeight - (_originalCharacterHeight - _characterController.height), cinemachineCameraTarget.transform.localPosition.z);
                yield return null;
            }
        }
        private IEnumerator StartDashCooldown()
        {
            yield return new WaitForSecondsRealtime(dashCooldown);
            isDashable = true;            
        }
        private IEnumerator StartDashChargeCooldown()
        {
            isInDashChargeCooldown = true;
            yield return new WaitForSecondsRealtime(dashChargeCooldown);
            isInDashChargeCooldown = false;
            dashCurrentCount += 1;
            this.PostEvent(EventID.onDashChargeCooldown, dashChargeCooldown);
        }

        private IEnumerator StartDashDuration()
        {
            isDashable = false;
            isInDashState = true;

            this.PostEvent(EventID.onPlayVFX, VFXID.dash);
            this.PostEvent(EventID.onPlaySound, AudioID.dash);
             this.PostEvent(EventID.onDash, dashCurrentCount);

            yield return new WaitForSecondsRealtime(dashDuration);

            this.PostEvent(EventID.onStopVFX, VFXID.dash);

            isInDashState = false;
            ResetDashDirection();
            StartCoroutine(StartDashCooldown());
        }
        private IEnumerator ChangeFOVWhileDash()
        {
            while (_cinemachineVirtualCamera.m_Lens.FieldOfView != _originalFOV * dashFOVMultiplier)
            {
                _cinemachineVirtualCamera.m_Lens.FieldOfView = Universal.Smoothing.LinearSmoothFixedTime(_cinemachineVirtualCamera.m_Lens.FieldOfView, _originalFOV, _originalFOV * dashFOVMultiplier, Time.unscaledDeltaTime, dashDuration);
                yield return null;
            }
        }

        private IEnumerator RevertFOVAfterDash()
        {
            while (_cinemachineVirtualCamera.m_Lens.FieldOfView != _originalFOV)
            {
                _cinemachineVirtualCamera.m_Lens.FieldOfView = Universal.Smoothing.LinearSmoothFixedTime(_cinemachineVirtualCamera.m_Lens.FieldOfView, _originalFOV * dashFOVMultiplier, _originalFOV, Time.unscaledDeltaTime, dashFOVRevertDuration);
                yield return null;
            }
        }

        private IEnumerator ChangeFOVWhileSlide()
        {
            while (_cinemachineVirtualCamera.m_Lens.FieldOfView != _originalFOV * dashFOVMultiplier)
            {
                _cinemachineVirtualCamera.m_Lens.FieldOfView = Universal.Smoothing.LinearSmoothFixedTime(_cinemachineVirtualCamera.m_Lens.FieldOfView, _originalFOV, _originalFOV * dashFOVMultiplier, Time.deltaTime, slideDuration);
                yield return null;
            }
        }

        private IEnumerator RevertFOVAfterSlide()
        {
            while (_cinemachineVirtualCamera.m_Lens.FieldOfView != _originalFOV)
            {
                _cinemachineVirtualCamera.m_Lens.FieldOfView = Universal.Smoothing.LinearSmoothFixedTime(_cinemachineVirtualCamera.m_Lens.FieldOfView, _originalFOV * dashFOVMultiplier, _originalFOV, Time.deltaTime, dashFOVRevertDuration);
                yield return null;
            }
        }


        private IEnumerator StartSlideDuration()
        {
            yield return new WaitForSecondsRealtime(slideDuration);
            SwitchToState("Crouch");
        }

        private IEnumerator StartLedgeGrabDuration()
        {
            yield return new WaitForSecondsRealtime(ledgeGrabDuration);
            SwitchToState("Idle");
        }
        #endregion

    }
}
