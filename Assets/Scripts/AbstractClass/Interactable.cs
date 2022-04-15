using System.Collections;
using UnityEngine;

namespace AbstractClass
{
    public abstract class Interactable : MonoBehaviour
    {
        public bool InteractableFlag;
        public abstract string GetDescription();
        public abstract void Interact();

        public bool isInteractable()
        {
            if (InteractableFlag)
                return true;
            else
                return false;
        }
    }
}