using AbstractClass;
using HighlightPlus;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    [RequireComponent(typeof(HighlightEffect))]
    public class KeyInteract : Interactable
    {
        private HighlightEffect _highlightEffect;

        private void Start()
        {
            _highlightEffect = GetComponent<HighlightEffect>();

            _highlightEffect.SetHighlighted(true);

            this.gameObject.SetActive(true);
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
                this.PostEvent(EventID.onSave);
                PlayerPrefs.SetInt(PlayerPrefEnum.Refresh.ToString(), 0);
                PlayerPrefs.SetInt(PlayerPrefEnum.CurrentScene.ToString(), SceneManager.GetActiveScene().buildIndex);
                PlayerPrefs.Save();
                this.gameObject.SetActive(false);
            }
        }
        public void SetInteractable(bool p_isInteractable)
        {
            isInteractable = p_isInteractable;
        }
        
        private void OnEnemyDead(int p_count)
        {
        }
    }
}