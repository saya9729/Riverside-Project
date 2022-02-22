using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	public class PlayerManager : MonoBehaviour
	{		
		//Manager
		[NonSerialized] public SelectionManager selectionManager;
		[NonSerialized] public ObjectManipulatorManager objectManipulatorManager;
		[NonSerialized] public InputManager inputManager;
		[NonSerialized] public PlayerStateManager playerStateManager;

		// select object
		[NonSerialized] public GameObject currentCenterScreenObject = null;

        private void Start()
        {
            selectionManager = GameObject.Find("SelectionManager").GetComponent<SelectionManager>();
            objectManipulatorManager = GameObject.Find("ObjectManipulatorManager").GetComponent<ObjectManipulatorManager>();
			
			//PlayerCapsule is the name of the object contain Player Input Component and cannot be change because of the Starter assets script
			inputManager = GameObject.Find("PlayerCapsule").GetComponent<InputManager>();
            
			playerStateManager = GameObject.Find("PlayerStateManager").GetComponent<PlayerStateManager>();
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

