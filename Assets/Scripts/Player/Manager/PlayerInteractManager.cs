using AbstractClass;
using UnityEngine;

namespace Player
{
    public class PlayerInteractManager : MonoBehaviour
    {
        public float interactionDistance;

        private Camera _cam;
        private Interactable _interactable;
        public TMPro.TextMeshProUGUI interactionText;

        private void Start()
        {
            _cam = Camera.main;
        }

        private void Update()
        {
            CheckInteraction();
        }

        private void CheckInteraction()
        {
            bool successfulHit = false;
            Ray ray = _cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                _interactable = hit.collider.GetComponent<Interactable>();

                if (_interactable != null)
                {
                    HandleInteraction();
                    interactionText.text = _interactable.GetDescription();
                    successfulHit = true;
                }
            }

            if (!successfulHit)
            {
                interactionText.text = "";
            }
        }

        private void HandleInteraction()
        {
            _interactable.Interact();
        }
    }
}