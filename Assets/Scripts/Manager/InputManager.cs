using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InputManager : MonoBehaviour
    {
        public bool interact;
        public float mouseScrollDelta;
        public Vector2 mousePosition;
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
    }
}
