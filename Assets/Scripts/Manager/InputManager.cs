using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InputManager : MonoBehaviour
    {
        [NonSerialized] public bool interact;
        [NonSerialized] public bool exit;
        [NonSerialized] public float mouseScrollDelta;
        [NonSerialized] public Vector2 mousePosition;
        [NonSerialized] public bool attack;
        [NonSerialized] public bool item;
        [NonSerialized] public bool crouch;

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
        public void OnAttack(InputValue p_value)
        {
            attack = p_value.isPressed;
        }
        public void OnItem(InputValue p_value)
        {
            item = p_value.isPressed;
        }
        public void OnCrouch(InputValue p_value)
        {
            crouch = p_value.isPressed;
        }

        public void OnExit(InputValue p_value)
        {
            exit = p_value.isPressed;
        }
    }
}
