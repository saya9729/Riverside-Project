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
            this.PostEvent(EventID.onStopSound, AudioID.ambience);
            this.PostEvent(EventID.onStopSound, AudioID.subAmbience);
            PlayerPrefs.SetInt(PlayerPrefEnum.CurrentScene.ToString(), SceneManager.GetActiveScene().buildIndex);
            PlayerPrefs.Save();
            this.PostEvent(EventID.onSave);
            SceneManager.LoadScene(0);
            Time.timeScale = 1f;
        }
    }
}