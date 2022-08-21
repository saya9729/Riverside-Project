using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class RedirectingNavigation : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private Selectable selectable;

    public void SelectObject()
    {
        if (selectable)
        {
            selectable.Select();
            selectable.GetComponent<HoverEffect>().EnableEffect(true);
        }
    }
    private void Start()
    {
        SelectObject();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SelectObject();
    }
    public void OnEnable()
    {
        SelectObject();
    }
    public void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            SelectObject();
        }
    }
}
