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

        private void Start()
        {
			SetPlayerManagerForAll(this);
        }

		void SetPlayerManagerForAll(PlayerManager p_playerManager)
        {
			selectionManager.SetPlayerManager(p_playerManager);
			playerStateManager.SetPlayerManager(p_playerManager);
        }


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

