using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerDeathSequence : MonoBehaviour
    {
        [SerializeField] private GameObject gameMenuCanvas;
        [SerializeField] private float deathTime = 2f;
        public void PlayPlayerDeathSequence()
        {
            DeadNotofier();
        }
        IEnumerator DeadNotofierCoroutine()
        {
            yield return new WaitForSecondsRealtime(deathTime);
            LoadSceneFromDeathSequence();
        }
        private void DeadNotofier()
        {
            gameMenuCanvas.SetActive(true);
            StartCoroutine(DeadNotofierCoroutine());
        }
        private void LoadSceneFromDeathSequence() 
        {
            gameMenuCanvas.SetActive(false);
            int scneneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.UnloadScene(scneneIndex);
            SceneManager.LoadScene(scneneIndex);
        }
    }
}
