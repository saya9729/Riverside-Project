using UnityEngine.SceneManagement;
using UnityEngine;

public class SaveOnTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider p_other)
    {
        if (p_other.CompareTag("Player"))
        {
            this.PostEvent(EventID.onSave);
        }
    }
}
