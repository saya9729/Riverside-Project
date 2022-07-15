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
    [RequireComponent(typeof(PlayerSkillManager))]

    [RequireComponent(typeof(PlayerMovementIdleState))]
    [RequireComponent(typeof(PlayerRunState))]
    [RequireComponent(typeof(PlayerDashState))]
    [RequireComponent(typeof(PlayerSlideState))]
    [RequireComponent(typeof(PlayerCrouchState))]
    [RequireComponent(typeof(PlayerWallRunState))]
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

        [SerializeField] private float dashDistanceWhileTimeSlowMultiflier = 1f;

        [SerializeField] private string playerPhaseLayerName = "PlayerPhase";        
        
        [SerializeField] private float dashFOVMultiplier = 1f;
        
        [SerializeField] private float dashFOVRevertDuration = 1f;

        [Space]
        [Header("Change rate")]
        [Tooltip("Acceleration and deceleration")]
        [SerializeField] private float speedChangeRate = 10.0f;
        [Tooltip("Rotation speed of the character")]
        [SerializeField] private float rotationSpeed = 1.0f;

        [SerializeField] private float airborneSteeringRate = 1f;

        [Space]
        [Header("Jump")]
        [Tooltip("The height the player can jump")]
        [SerializeField] private float jumpHeight = 1.2f;
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

        [SerializeField] private Vector3 groundedBoxDimention = new Vector3(1, 1, 1);
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
        [SerializeField] private GameObject particleDash;

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

        private float currentGravity = -15.0f;

        private bool isAllowToJump = true;
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
        private float _originalFOV = 90f;

        private IEnumerator _slideCoroutine;
        private IEnumerator _crouchDownCoroutine;
        private IEnumerator _standUpCoroutine;
        private IEnumerator _changeFOVWhileDashCoroutine;
        private IEnumerator _revertFOVAfterDashCoroutine;

        private PlayerInput _playerInput;
        private CharacterController _characterController;
        public InputManager inputManager;
        private PlayerSkillManager _playerSkillManager;
        private CapsuleCollider _characterCapsuleCollider;
        private PlayerActionStateManager _playerActionStateManager;
        private CinemachineVirtualCamera _cinemachineVirtualCamera;

        private const float _threshold = 0.01f;

        private bool IsCurrentDeviceMouse => _playerInput.currentControlScheme == "KeyboardMouse";

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
            _characterController = GetComponent<CharacterController>();
            _playerInput = GetComponent<PlayerInput>();
            inputManager = GetComponent<InputManager>();
            _playerSkillManager = GetComponentInChildren<PlayerSkillManager>();
            _characterCapsuleCollider = GetComponent<CapsuleCollider>();
            _playerActionStateManager = GetComponent<PlayerActionStateManager>();
        }

        protected override void InitializeVariable()
        {
            EnableRunGravity();
            //register listener
            this.RegisterListener(EventID.onDodgePress, (param) => onDodgePress());

            //set original controller value
            _originalCharacterHeight = _characterController.height;
            _originalCharacterCenter = _characterController.center;
            _originalCamHolderHeight = cinemachineCameraTarget.transform.localPosition.y;

            _cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            _originalFOV = _cinemachineVirtualCamera.m_Lens.FieldOfView;

            _originalPlayerLayer = gameObject.layer;

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

            if (isRoofed)
            {
                Gizmos.color = transparentGreen;
            }
            else
            {
                Gizmos.color = transparentRed;
            }
            Vector3 rayPosition = new Vector3(transform.position.x, transform.position.y + _characterController.height + roofedOffset, transform.position.z);
            Gizmos.DrawRay(rayPosition, Vector3.up * roofedDistance);


        }
        #endregion

        #region API
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
        public void DisableJump()
        {
            isAllowToJump = false;
        }
        public void EnableJump()
        {
            isAllowToJump = true;
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
        public void SetAirborneInertiaDirectionWhileDoubleJump()
        {
            if (inputDirection != Vector3.zero)
            {
                airborneInertiaDirection = inputDirection;
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
            _characterController.Move(
                    inputDirection * currentSpeed * Time.deltaTime
                    + Vector3.up * verticalVelocity * Time.deltaTime);
        }
        public void MoveWhileAirborne()
        {
            HandleAirborneSteering();

            _characterController.Move(
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
            _characterController.Move(
                    slideDirection * currentSpeed * Time.deltaTime
                    + Vector3.up * verticalVelocity * Time.deltaTime);
        }
        public void MoveWhileDash()
        {
            _characterController.Move(dashDirection * dashSpeed * Time.unscaledDeltaTime);
        }
        public void MoveWhileWallRun()
        {

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
            if (isAllowToJump && !isRoofed)
            {
                if (isGrounded && inputManager.IsButtonDownThisFrame("Jump"))
                {
                    DisableSlideGravity();
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
                    AudioInterface.PlayAudio("secondJump");
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
            //smooth the speed change (momentum mechanic) 
            float inputMagnitude = inputManager.analogMovement ? inputManager.move.magnitude : 1f;
            currentSpeed = Universal.Smoothing.LinearSmoothFixedRate(currentSpeed, targetSpeed * inputMagnitude, speedChangeRate * Time.deltaTime);
            //currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed * inputMagnitude, Time.deltaTime * speedChangeRate);
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
        //    isAllowToJump = false;
        //    yield return new WaitForSeconds(jumpCooldown);
        //    isAllowToJump = true;
        //}
        //public void StartCoroutineStartJumpCooldown()
        //{
        //    StartCoroutine(StartJumpCooldown());
        //}
        private IEnumerator CrouchDown()
        {
            while (_characterController.height > crouchHeight)
            {
                _characterController.height = Universal.Smoothing.LinearSmoothFixedTime(_characterController.height, _originalCharacterHeight, crouchHeight, Time.deltaTime, timeToCrouch);
                _characterController.center = Universal.Smoothing.LinearSmoothFixedTime(_characterController.center, _originalCharacterCenter, crouchCenter, Time.deltaTime, timeToCrouch);

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
                _characterController.height = Universal.Smoothing.LinearSmoothFixedTime(_characterController.height, crouchHeight, _originalCharacterHeight, Time.deltaTime, timeToCrouch);
                _characterController.center = Universal.Smoothing.LinearSmoothFixedTime(_characterController.center, crouchCenter, _originalCharacterCenter, Time.deltaTime, timeToCrouch);

                _characterCapsuleCollider.height = _characterController.height;
                _characterCapsuleCollider.center = _characterController.center;

                cinemachineCameraTarget.transform.localPosition = new Vector3(cinemachineCameraTarget.transform.localPosition.x, _originalCamHolderHeight - (_originalCharacterHeight - _characterController.height), cinemachineCameraTarget.transform.localPosition.z);
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
            if (_playerSkillManager.gameIsSlowDown)
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
        private IEnumerator ChangeFOVWhileDash()
        {
            while (_cinemachineVirtualCamera.m_Lens.FieldOfView != _originalFOV * dashFOVMultiplier)
            {
                _cinemachineVirtualCamera.m_Lens.FieldOfView = Universal.Smoothing.LinearSmoothFixedTime(_cinemachineVirtualCamera.m_Lens.FieldOfView, _originalFOV, _originalFOV * dashFOVMultiplier, Time.deltaTime, dashDuration);
                yield return null;
            }
        }
        
        private IEnumerator RevertFOVAfterDash()
        {
            while (_cinemachineVirtualCamera.m_Lens.FieldOfView != _originalFOV)
            {
                _cinemachineVirtualCamera.m_Lens.FieldOfView = Universal.Smoothing.LinearSmoothFixedTime(_cinemachineVirtualCamera.m_Lens.FieldOfView, _originalFOV * dashFOVMultiplier, _originalFOV, Time.deltaTime, dashFOVRevertDuration);
                yield return null;
            }
        }


        private IEnumerator StartSlideDuration()
        {
            yield return new WaitForSeconds(slideDuration);
            SwitchToState("Crouch");
        }
        #endregion

    }
}
