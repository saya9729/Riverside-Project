using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerLoseSequence : MonoBehaviour
    {
        [SerializeField] private GameObject gameMenuCanvas;
        [SerializeField] private float loseTime = 2f;
        public void PlayPlayerLoseSequence()
        {
            LoseAnnouncement();
            StopComponent();
        }
        IEnumerator LoseAnnouncementCoroutine()
        {
            yield return new WaitForSecondsRealtime(loseTime);
            LoadSceneFromLoseSequence();
        }
        private void LoseAnnouncement()
        {
            gameMenuCanvas.SetActive(true);
            StartCoroutine(LoseAnnouncementCoroutine());
        }
        private void LoadSceneFromLoseSequence() 
        {
            gameMenuCanvas.SetActive(false);
            int scneneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(scneneIndex);
        }
        private void StopComponent()
        {
            Time.timeScale = 1f;
            this.PostEvent(EventID.onStopSound, AudioID.timeskill);
        }
    }
}
