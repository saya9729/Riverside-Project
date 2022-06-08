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

        public void QuitButton()
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
}