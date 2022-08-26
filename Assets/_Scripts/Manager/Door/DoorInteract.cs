using AbstractClass;
using HighlightPlus;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace Player
{
    [RequireComponent(typeof(HighlightEffect))]
    public class DoorInteract : Interactable
    {
        [Tooltip("Number of key need to open this door")]
        [SerializeField] private KeyInteract[] keyInteractArray;
        public GameObject[] platform;

        private HighlightEffect _highlightEffect;
        private int _countKey = 0;
        private int _playerCurrentLevel = 0;

        private void Start()
        {
            _highlightEffect = GetComponent<HighlightEffect>();
            _playerCurrentLevel = PlayerPrefs.GetInt(PlayerPrefEnum.CurrentScene.ToString(), SceneManager.GetActiveScene().buildIndex);
            isInteractable = false;
            _highlightEffect.SetHighlighted(true);
            this.RegisterListener(EventID.onKeyCollected, (param) => OnKeyCollected((int)param));
            this.RegisterListener(EventID.onSave, (param) => SaveKeyInteractState());
            this.RegisterListener(EventID.onRefresh, (param) => RefreshEnvironmentData());

            LoadKeyInteractState();
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
                PlayerPrefs.SetInt(PlayerPrefEnum.Refresh.ToString(), 1);
                PlayerPrefs.Save();
                this.PostEvent(EventID.onWin);
                _highlightEffect.SetHighlighted(false);
                isInteractable = false;
            }
        }

        private void OnKeyCollected(int p_count)
        {
            _countKey += p_count;
            if (_countKey == keyInteractArray.Length)
            {
                isInteractable = true;
                for(int i = 0; i < platform.Length; i++)
                {
                    platform[i].SetActive(!platform[i].activeSelf);
                }
            }
        }
        public void SaveKeyInteractState()
        {
            SaveManager.SaveEnvironment(keyInteractArray, _playerCurrentLevel);
        }
        public void LoadKeyInteractState()
        {
            if (keyInteractArray == null)
            {
                Debug.LogWarning("There are no keys link to door");
                return;
            }
            EnvironmentData environmentData = SaveManager.LoadEnvironment(_playerCurrentLevel);

            bool refresh = Convert.ToBoolean(PlayerPrefs.GetInt(PlayerPrefEnum.Refresh.ToString(), 0));

            if (environmentData == null || refresh)
            {
                Debug.Log("generate default value");
                RefreshEnvironmentData();
                return;
            }
            for (int i = 0 ; i< environmentData.isKeyInteractableArray.Length; i++)
            {
                keyInteractArray[i].SetInteractable(environmentData.isKeyInteractableArray[i]);
                if (!keyInteractArray[i].IsInteractable())
                {
                    OnKeyCollected(1);
                }
                keyInteractArray[i].gameObject.SetActive(environmentData.isKeyInteractableArray[i]);
            }
        }
        public void RefreshEnvironmentData()
        {
            if(keyInteractArray == null)
            {
                Debug.LogWarning("There are no keys link to door");
                return; 
            }
            foreach (KeyInteract key in keyInteractArray)
            {
                key.SetInteractable(true);
            }
            SaveKeyInteractState();
        }
    }
}