using UnityEngine;
using UnityEngine.UI;

public class ToggleChangeEffect : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text toggleText;
    public void SetEffect(bool p_isOn)
    {
        if (p_isOn)
        {
            toggleText.text = "On";
        }
        else 
        {
            toggleText.text = "Off";
        }
    }
}
