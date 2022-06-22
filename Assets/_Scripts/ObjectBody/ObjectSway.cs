using System;
using UnityEngine;

namespace Player
{
    public class ObjectSway : MonoBehaviour
    {
        private InputManager inputManager;

        [Header("Sway Settings")]
        [SerializeField] private float smooth = 5;
        [SerializeField] private float multiplier = 3;

        void Start()
        {
            inputManager = GetComponentInParent<InputManager>();
        }

        private void Update()
        {
            // get mouse input
            float mouseX = inputManager.look.x * multiplier;
            float mouseY = inputManager.look.y * multiplier;

            // calculate target rotation
            Quaternion rotationX = Quaternion.AngleAxis(-mouseX, Vector3.up);
            Quaternion rotationY = Quaternion.AngleAxis(-mouseY, Vector3.right);

            Quaternion targetRotation = rotationX * rotationY;

            // rotate 
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
        }
    }
}