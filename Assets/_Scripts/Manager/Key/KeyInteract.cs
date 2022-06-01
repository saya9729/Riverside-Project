using AbstractClass;
using HighlightPlus;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(HighlightEffect))]
    public class KeyInteract : Interactable
    {
        private HighlightEffect _highlightEffect;

        private void Start()
        {
            _highlightEffect = GetComponent<HighlightEffect>();

            isInteractable = true;
            _highlightEffect.SetHighlighted(true);
        }

        public override string GetDescription()
        {
            if (isInteractable)
            {
                return "[E]";
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
                this.PostEvent(EventID.onKeyCollected, 1);
            }
        }

        private void OnEnemyDead(int p_count)
        {
        }
    }
}