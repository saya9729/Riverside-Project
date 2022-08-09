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
            this.PostEvent(EventID.onPlaySound, AudioID.ambience);
            this.PostEvent(EventID.onPlaySound, AudioID.backgroundmusic);
            ToggleContinue();
        }
        public void NewGame()
        {
            for (int sceneIndex = 1; sceneIndex < SceneManager.sceneCountInBuildSettings; sceneIndex++)
            {
                SaveManager.DeletePlayer(sceneIndex);
                SaveManager.DeleteEnvironment(sceneIndex);
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
            if (!SaveManager.FileSavePlayerExist(PlayerPrefs.GetInt("CurrentScene", 0)))
            {
                continueButton.interactable = false;
                return;
            }
            continueButton.interactable = true;
        }
    }
}