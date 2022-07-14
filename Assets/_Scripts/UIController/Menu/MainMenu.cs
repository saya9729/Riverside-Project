using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace GameUI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        public void Start()
        {
            AudioInterface.PlayAudio("ambience");
            AudioInterface.PlayAudio("backgroundmusic");
            ToggleContinue();
        }
        public void NewGame()
        {
            for (int sceneIndex = 1; sceneIndex < SceneManager.sceneCount; sceneIndex++)
            {
                SaveManager.DeletePlayer(sceneIndex);
            }
            PlayerPrefs.DeleteKey("CurrentScene");
            PlayerPrefs.Save();
            StartGame();
        }
        public void StartGame()
        {
            int nextScene = PlayerPrefs.GetInt("CurrentScene", SceneManager.GetActiveScene().buildIndex + 1);
            SceneManager.LoadScene(nextScene);
            PlayerPrefs.SetInt("CurrentScene", nextScene);
            PlayerPrefs.Save();
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void QuitButton()
        {
            Debug.Log("Quit");
            Application.Quit();
        }

        public void ToggleContinue()
        {
            if (!SaveManager.FileSaveExist(PlayerPrefs.GetInt("CurrentScene", 0)))
            {
                continueButton.interactable = false;
                return;
            }
            continueButton.interactable = true;
        }
    }
}