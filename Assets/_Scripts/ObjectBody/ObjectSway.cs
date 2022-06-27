using System;
using UnityEngine;
using Footsteps;

namespace Player
{
    public class ObjectSway : MonoBehaviour
    {
        private InputManager inputManager;
        private CharacterController player;

        [Header("Sway Settings")]
        [SerializeField] private float smooth = 5;
        [SerializeField] private float multiplier = 3;
        [SerializeField] private float triggerVelocity = 0.5f;
        [SerializeField] private float jumpOffset = 7;

        void Start()
        {
            inputManager = GetComponentInParent<InputManager>();
            player = GetComponentInParent<CharacterController>();
        }

        private void Update()
        {
            // get mouse input
            float mouseX = inputManager.look.x * multiplier;
            float mouseY = inputManager.look.y * multiplier;

            if(player.velocity.y >= triggerVelocity)
            {
                mouseY -= jumpOffset;
            }

            if(player.velocity.y <= -triggerVelocity)
            {
                mouseY += jumpOffset;
            }

            // calculate target rotation
            Quaternion rotationX = Quaternion.AngleAxis(-mouseX, Vector3.up);
            Quaternion rotationY = Quaternion.AngleAxis(-mouseY, Vector3.right);

            Quaternion targetRotation = rotationX * rotationY;

            // rotate 
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
        }
    }
}