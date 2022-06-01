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
            PlayerPrefs.SetInt("CurrentScene", SceneManager.GetActiveScene().buildIndex);
            PlayerPrefs.Save();
            SceneManager.LoadScene(0);
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
