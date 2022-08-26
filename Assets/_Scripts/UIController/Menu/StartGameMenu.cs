using UnityEngine.SceneManagement;
using UnityEngine;

public class StartGameMenu : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.SetInt(PlayerPrefEnum.Refresh.ToString(), 0);
        PlayerPrefs.Save();
    }

    public void NewGame()
    {
        DeleteSaveGame();
        StartChapter(1);
    }

    public void Continue()
    {
        int nextScene = PlayerPrefs.GetInt(PlayerPrefEnum.CurrentScene.ToString(), SceneManager.GetActiveScene().buildIndex + 1);
        StartChapter(nextScene);
    }

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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void RefeshChapter()
    {
        PlayerPrefs.SetInt(PlayerPrefEnum.Refresh.ToString(), 1);
        PlayerPrefs.Save();
    }

}
