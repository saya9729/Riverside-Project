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
            StartGame();
            SaveManager.DeletePlayer();
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
            Debug.Log(SaveManager.FileSaveExist());
            if (!SaveManager.FileSaveExist())
            {
                continueButton.interactable = false;
                return;
            }
            continueButton.interactable = true;
        }
    }
}