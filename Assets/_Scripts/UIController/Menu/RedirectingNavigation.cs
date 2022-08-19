using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class RedirectingNavigation : Selectable
{

    [SerializeField] private Slider slider;

    protected override void Start()
    {
        slider.Select();
        base.Start();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        slider.Select();
    }
}
