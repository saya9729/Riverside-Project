using AbstractClass;
using HighlightPlus;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(HighlightEffect))]
    public class DoorInteract : Interactable
    {
        [Tooltip("Number of key need to open this door")]
        [SerializeField] private KeyInteract[] keyInteract;
        public GameObject[] platform;

        private HighlightEffect _highlightEffect;
        private int _countKey = 0;

        private void Start()
        {
            _highlightEffect = GetComponent<HighlightEffect>();

            isInteractable = false;
            _highlightEffect.SetHighlighted(true);
            this.RegisterListener(EventID.onKeyCollected, (param) => OnKeyCollected((int)param));
            foreach (KeyInteract key in keyInteract)
            {
                if (key.IsInteractable())
                {
                    isInteractable = false;
                }
                else
                {
                    OnKeyCollected(1);
                }
            }
        }

        public override string GetDescription()
        {
            if (isInteractable)
            {
                return "[E] Interact with Door";
            }
            else
            {
                return "";
            }
        }

        public override void Interact()
        {
            if (isInteractable)
            {
                _highlightEffect.SetHighlighted(false);
                isInteractable = false;
            }
        }

        private void OnKeyCollected(int p_count)
        {
            _countKey += p_count;
            Debug.Log("count key " + _countKey);

            if (_countKey == keyInteract.Length)
            {
                isInteractable = true;
                platform[0].SetActive(!platform[0].activeSelf);
            }
        }
    }
}