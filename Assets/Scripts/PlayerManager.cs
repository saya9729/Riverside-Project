using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	public class PlayerManager : MonoBehaviour
	{
		[Header("Manager")]
		[SerializeField] private SelectionManager selectionManager;
		[SerializeField] private ObjectManipulatorManager objectManipulatorManager;
		[SerializeField] private InputManager inputManager;

		// select objecy
		private GameObject _currentCenterScreenObject = null;

		// Update is called once per frame
		void Update()
		{
			UpdateCenterScreenObject();
			Interact();
		}

		private void UpdateCenterScreenObject()
		{
			_currentCenterScreenObject = selectionManager.GetObjectAtScreenCenter();
		}

		private void Interact()
		{
			bool mousePressed = inputManager.Player.Interact.IsPressed();
			float mouseScrollDelta;
			if (mousePressed)
			{
				objectManipulatorManager.PickUpObject(_currentCenterScreenObject);
				mouseScrollDelta = inputManager.Player.Rotate.ReadValue<float>();
				//if (inputManager.rotate == Vector2.zero)
				//{
				//	mouseScrollDelta = 0.0f;
				//}
				//else
				//{
				//	mouseScrollDelta = inputManager.rotate.y;
				//}
				objectManipulatorManager.RotateFromMouseWheel(mouseScrollDelta);
			}
			else
			{
				objectManipulatorManager.DropObject();
			}
		}
	}
}

