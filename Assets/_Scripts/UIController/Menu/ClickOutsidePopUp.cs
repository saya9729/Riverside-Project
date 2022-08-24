using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class ClickOutsidePopUp : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Button buttonNo;
    public void OnPointerDown(PointerEventData p_eventData)
    {
        buttonNo.onClick.Invoke();
    }
}
