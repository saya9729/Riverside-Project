using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private float jumpBufferTime = 0.1f;

        [NonSerialized] public PlayerInput _playerInput;

        //public bool interact;
        [NonSerialized] public bool exit = false;
        [NonSerialized] public bool usingPocketWatch = false;
        [NonSerialized] public bool primaryLightAttack = false;
        [NonSerialized] public bool secondaryAttack = false;
        [NonSerialized] public float mouseScrollDelta = 0;
        [NonSerialized] public Vector2 mousePosition = Vector2.zero;
        [NonSerialized] public bool useHealthPot = false;
        [NonSerialized] public bool menu = false;
        [NonSerialized] public bool pullFromSol = false;
        [NonSerialized] public bool switchAim = false;
        [NonSerialized] public bool cooldownWeapon = false;

        [Header("Character Input Values")]

        [NonSerialized] public Vector2 move = Vector2.zero;
        [NonSerialized] public Vector2 look = Vector2.zero;
        [NonSerialized] public bool jump = false;
        [NonSerialized] public bool dash = false;
        [NonSerialized] public bool crouch = false;

        [Header("Movement Settings")]
        [NonSerialized] public bool analogMovement = false;

        [Header("Interact Input")]
        [NonSerialized] public bool interact = false;
        [NonSerialized] public bool pressInteractDown = false;

#if !UNITY_IOS || !UNITY_ANDROID

        [Header("Mouse Cursor Settings")]
        [NonSerialized] public bool cursorLocked = true;

        [NonSerialized] public bool cursorInputForLook = true;
#endif

        private IEnumerator _jumpBufferCoroutine;

        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnJump(InputValue value)
        {
            if (value.isPressed)
            {
                jump = true;
                try
                {
                    StopCoroutine(_jumpBufferCoroutine);
                }
                catch
                {

                }
            }
            else
            {
                _jumpBufferCoroutine = StartJumpBuffer();
                StartCoroutine(_jumpBufferCoroutine);
            }
        }
        private IEnumerator StartJumpBuffer()
        {
            yield return new WaitForSecondsRealtime(jumpBufferTime);
            jump = false;
        }

        public void OnDash(InputValue value)
        {
            dash = value.isPressed;
        }

        private void OnCrouch(InputValue value)
        {
            crouch = value.isPressed;
        }

        private void OnUseHealthPotion(InputValue value)
        {
            useHealthPot = value.isPressed;
        }

        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void OnInteract(InputValue p_value)
        {
            interact = p_value.isPressed;
        }

        public void OnRotate(InputValue p_value)
        {
            mouseScrollDelta = p_value.Get<Vector2>().y;
        }

        public void OnMousePosition(InputValue p_value)
        {
            mousePosition = p_value.Get<Vector2>();
        }

        public void OnExit(InputValue p_value)
        {
            exit = p_value.isPressed;
        }

        public void OnUsingPocketWatch(InputValue p_value)
        {
            usingPocketWatch = p_value.isPressed;
        }
        public void OnPrimaryLightAttack(InputValue p_value)
        {
            primaryLightAttack = p_value.isPressed;
        }
        public void OnSecondaryAttack(InputValue p_value)
        {
            secondaryAttack = p_value.isPressed;
        }

        public void OnMenu(InputValue p_value)
        {
            menu = p_value.isPressed;
        }

        public void OnPullFromSol(InputValue p_value)
        {
            pullFromSol = p_value.isPressed;
        }

        public void OnSwitchAim(InputValue p_value)
        {
            switchAim = p_value.isPressed;
        }

        public void OnCooldownWeapon(InputValue p_value)
        {
            cooldownWeapon = p_value.isPressed;
        }

#if !UNITY_IOS || !UNITY_ANDROID

        private void OnApplicationFocus(bool hasFocus)
        {
            if (_playerInput.inputIsActive)
            { 
                SetCursorState(cursorLocked); 
            }
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }

#endif

        private void Start()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        public bool IsButtonDownThisFrame(string name)
        {
            return _playerInput.actions[name].WasPressedThisFrame();
        }
    }


}