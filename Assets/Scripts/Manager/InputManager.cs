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
        [NonSerialized] public bool usingPocketWatch;
        [NonSerialized] public float mouseScrollDelta;
        [NonSerialized] public Vector2 mousePosition;
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
    }
}
