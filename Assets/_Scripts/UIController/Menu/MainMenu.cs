using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameUI
{
    public class MainMenu : MonoBehaviour
    {
        public void QuitButton()
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
}