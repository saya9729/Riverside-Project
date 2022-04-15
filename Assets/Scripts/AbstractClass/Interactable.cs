using System.Collections;
using UnityEngine;

namespace AbstractClass
{
    public abstract class Interactable : MonoBehaviour
    {

        public abstract string GetDescription();
        public abstract void Interact();

    }
}