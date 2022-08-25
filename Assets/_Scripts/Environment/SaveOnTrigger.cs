using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveOnTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider p_other)
    {
        if (p_other.CompareTag("Player"))
        {
            this.PostEvent(EventID.onSave);
            PlayerPrefs.SetInt(PlayerPrefEnum.Refresh.ToString(), 0);
            PlayerPrefs.SetInt(PlayerPrefEnum.CurrentScene.ToString(), SceneManager.GetActiveScene().buildIndex);
            PlayerPrefs.Save();
        }
    }
}
