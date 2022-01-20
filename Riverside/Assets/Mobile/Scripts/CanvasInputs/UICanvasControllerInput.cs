using UnityEngine;

namespace FirstPerson
{
    public class UICanvasControllerInput : MonoBehaviour
    {

        [Header("Output")]
        public FirstPersonInputs FirstPersonInputs;

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            FirstPersonInputs.MoveInput(virtualMoveDirection);
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            FirstPersonInputs.LookInput(virtualLookDirection);
        }

        public void VirtualJumpInput(bool virtualJumpState)
        {
            FirstPersonInputs.JumpInput(virtualJumpState);
        }

        public void VirtualSprintInput(bool virtualSprintState)
        {
            FirstPersonInputs.SprintInput(virtualSprintState);
        }
        
    }

}
