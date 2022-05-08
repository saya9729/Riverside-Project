using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameUI
{
    public class GameMenu : MonoBehaviour
    {
        public void QuitGameToMainMenu()
        {
            PlayerPrefs.SetInt("CurrentScene", SceneManager.GetActiveScene().buildIndex);
            PlayerPrefs.Save();
            SceneManager.LoadScene(0);
        }
    }
}