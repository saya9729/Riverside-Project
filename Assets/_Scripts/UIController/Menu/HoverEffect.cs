using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Image image;

    public void EnableEffect(bool p_enable)
    {
        if (image)
        {
            image.enabled = p_enable;
        }
    }
    private void Start()
    {
        EnableEffect(false);
    }
    public void OnPointerEnter(PointerEventData p_eventData)
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
        EnableEffect(true);
    }

    public void OnPointerExit(PointerEventData p_eventData)
    {
    }
    public void OnSelect(BaseEventData p_eventData)
    {
        EnableEffect(true);
    }
    public void OnDeselect(BaseEventData p_eventData)
    {
        EnableEffect(false);
    }
    public void OnDisable()
    {
        EnableEffect(false);
    }
}
