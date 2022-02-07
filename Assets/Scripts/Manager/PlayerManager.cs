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
			if (inputManager.interact)
			{
				objectManipulatorManager.PickUpObject(_currentCenterScreenObject);                
                objectManipulatorManager.RotateFromMouseWheel(inputManager.mouseScrollDelta);
			}
			else
			{
				objectManipulatorManager.DropObject();
			}
		}
	}
}

