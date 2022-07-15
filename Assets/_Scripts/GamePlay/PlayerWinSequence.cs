using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerWinSequence : MonoBehaviour
    {
        [SerializeField] private GameObject gameMenuCanvas;
        [SerializeField] private float winTime = 2f;
        public void PlayPlayerWinSequence()
        {
            WinAnnouncement();
            StopComponent();
        }
        IEnumerator WinAnnouncementCoroutine()
        {
            yield return new WaitForSecondsRealtime(winTime);
            LoadSceneFromWinSequence();
        }
        private void WinAnnouncement()
        {
            gameMenuCanvas.SetActive(true);
            StartCoroutine(WinAnnouncementCoroutine());
        }
        private void LoadSceneFromWinSequence() 
        {
            gameMenuCanvas.SetActive(false);
            if ((SceneManager.GetActiveScene().buildIndex + 1) < SceneManager.sceneCountInBuildSettings)
            {
                PlayerPrefs.SetInt("CurrentScene", SceneManager.GetActiveScene().buildIndex + 1);
            }
            else 
            { 
                PlayerPrefs.SetInt("CurrentScene", 0);
                Cursor.lockState = CursorLockMode.None;
            }
            PlayerPrefs.Save();
            SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentScene", 0));
        }
        private void StopComponent()
        {
            Time.timeScale = 1f;
            AudioInterface.StopAudio("timeskill");
        }
    }
}
