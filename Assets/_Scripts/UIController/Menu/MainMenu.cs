using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace GameUI
{
    public class MainMenu : MonoBehaviour
    {
        public void StartGame()
        {
            AudioInterface.PlayAudio("ambience");
            int nextScene = PlayerPrefs.GetInt("CurrentScene", SceneManager.GetActiveScene().buildIndex + 1);
            SceneManager.LoadScene(nextScene);
            PlayerPrefs.SetInt("CurrentScene", nextScene);
            PlayerPrefs.Save();
            Cursor.lockState = CursorLockMode.Locked;
        }

        [Header("Levels to Load")]
        public string _newGameLevel;
        private string _levelToLoad;
        [SerializeField] private GameObject noSavedGameDialog = null;

        public void NewGameDialogYes()
        {
            SceneManager.LoadScene(_newGameLevel);
        }

        public void LoadGameDialogYes()
        {
            if (PlayerPrefs.HasKey("SavedLevel"))
            {
                _levelToLoad = PlayerPrefs.GetString("SavedLevel");
                SceneManager.LoadScene(_levelToLoad);
            }
            else
            {
                noSavedGameDialog.SetActive(true);
            }
                
        }

        public void ExitButton()
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
}