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
            if ((SceneManager.GetActiveScene().buildIndex + 2) < SceneManager.sceneCountInBuildSettings)
            {
                PlayerPrefs.SetInt(PlayerPrefEnum.CurrentScene.ToString(), SceneManager.GetActiveScene().buildIndex + 1);
            }
            else 
            {
                PlayerPrefs.DeleteKey(PlayerPrefEnum.CurrentScene.ToString());
                Cursor.lockState = CursorLockMode.None;
            }
            PlayerPrefs.Save();
            SceneManager.LoadScene(PlayerPrefs.GetInt(PlayerPrefEnum.CurrentScene.ToString(), 0));
        }
        private void StopComponent()
        {
            Time.timeScale = 1f;
            this.PostEvent(EventID.onStopSound, AudioID.timeskill);
        }
    }
}
