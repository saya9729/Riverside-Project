using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InputManager : MonoBehaviour
    {        
        //public bool interact;
        public bool exit;
        public bool usingPocketWatch;
        public bool primaryLightAttack;
        public bool secondaryAttack;
        public float mouseScrollDelta;
        public Vector2 mousePosition;
        public bool useHealthPot;
        public bool menu;

        [Header("Character Input Values")]

        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool dash;
        public bool crouch;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Interact Input")]
        public bool interact;
        public bool pressInteractDown;

#if !UNITY_IOS || !UNITY_ANDROID

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;

        public bool cursorInputForLook = true;
#endif

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
            JumpInput(value.isPressed);
        }

        public void OnDash(InputValue value)
        {
            dash = value.isPressed;
            this.PostEvent(EventID.onDodgePress);
        }

        private void OnCrouch(InputValue value)
        {
            crouch = value.isPressed;
        }

        void OnUseHealthPotion(InputValue value)
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

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
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

#if !UNITY_IOS || !UNITY_ANDROID

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }

#endif
    }
}