using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameUI
{
    public class MainMenu : MonoBehaviour
    {
        public void DeleteSaveGame()
        {
            for (int sceneIndex = 1; sceneIndex < SceneManager.sceneCountInBuildSettings; sceneIndex++)
            {
                SaveManager.DeletePlayer(sceneIndex);
                SaveManager.DeleteEnvironment(sceneIndex);
            }
            PlayerPrefs.DeleteKey(PlayerPrefEnum.CurrentScene.ToString());
            PlayerPrefs.Save();
        }
        public void StartChapter(int p_chapter)
        {
            SceneManager.LoadScene(p_chapter);
            PlayerPrefs.SetInt(PlayerPrefEnum.CurrentScene.ToString(), p_chapter);
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