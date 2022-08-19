using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Image image;

    private void Start()
    {
        if (image != null)
        {
            image.enabled = false;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.enabled = false;
    }
    public void OnSelect(BaseEventData eventData)
    {
        image.enabled = true;
    }
    public void OnDeselect(BaseEventData eventData)
    {
        image.enabled = false;
    }
}
