using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	public class PlayerManager : MonoBehaviour
	{
		[Header("Manager")]
		public SelectionManager selectionManager;
		public ObjectManipulatorManager objectManipulatorManager;
		public InputManager inputManager;
		public PlayerStateManager playerStateManager;

		// select object
		[NonSerialized] public GameObject currentCenterScreenObject = null;

		// Update is called once per frame
		void Update()
		{
			UpdateCenterScreenObject();			
		}

		private void UpdateCenterScreenObject()
		{
			currentCenterScreenObject = selectionManager.GetObjectAtScreenCenter();
		}
	}
}

