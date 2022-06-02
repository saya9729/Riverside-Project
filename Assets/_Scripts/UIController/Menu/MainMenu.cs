using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameUI
{
    public class MainMenu : MonoBehaviour
    {
        public void StartGame()
        {
            int nextScene = PlayerPrefs.GetInt("CurrentScene", SceneManager.GetActiveScene().buildIndex + 1);
            SceneManager.LoadScene(nextScene);
            PlayerPrefs.SetInt("CurrentScene", nextScene);
            PlayerPrefs.Save();
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void QuitGame()
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
}