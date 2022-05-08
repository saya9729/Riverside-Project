using AbstractClass;
using HighlightPlus;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Player
{
    public class PlayerInteractManager : MonoBehaviour
    {
        public float interactionDistance;
        public TMPro.TextMeshProUGUI interactionText;

        private Camera _cam;
        private Interactable _interactable;
        private HighlightEffect _highlightEffect;
        private PlayerStateManager _playerStateManager;
        private PlayerInput _playerInput;

        private void Start()
        {
            _cam = Camera.main;
            _highlightEffect = GetComponent<HighlightEffect>();
            _playerStateManager = GetComponent<PlayerStateManager>();
            _playerInput = GetComponentInParent<PlayerInput>();
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
            //if (_playerStateManager.inputManager.interact)
            //{
            //    _interactable.Interact();
            //}
            //if (InputProxy.GetKeyDown("E"))
            //{
            //    _interactable.Interact();
            //}
            if (_playerInput.actions["Interact"].WasPressedThisFrame())
            {
                _interactable.Interact();
            }
        }
    }
}