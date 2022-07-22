using System.Collections;
using UnityEngine;

namespace AbstractClass
{
    public abstract class Interactable : MonoBehaviour
    {
        protected bool isInteractable = true;
        public abstract string GetDescription();
        public abstract void Interact();

        public bool IsInteractable()
        {
            if (isInteractable)
                return true;
            else
                return false;
        }
    }
}