using AbstractClass;
using HighlightPlus;
using UnityEngine;

[RequireComponent(typeof(HighlightEffect))]
public class DoorInteract : Interactable
{
    [Tooltip("Number of key need to open this door")]
    public int keyNumbers = 3;
    public GameObject[] platform;

    private HighlightEffect _highlightEffect;
    private int _countKey = 0;

    private void Start()
    {
        _highlightEffect = GetComponent<HighlightEffect>();

        isInteractable = false;
        _highlightEffect.SetHighlighted(true);
        this.RegisterListener(EventID.onKeyCollected, (param) => OnKeyCollected((int)param));
    }

    public override string GetDescription()
    {
        if (isInteractable)
        {
            return "[E] Interact with Door";
        }
        else
        {
            return "";
        }
    }

    public override void Interact()
    {
        if (isInteractable)
        {
            _highlightEffect.SetHighlighted(false);
            isInteractable = false;
        }
    }

    private void OnKeyCollected(int p_count)
    {
        _countKey += p_count;
        Debug.Log("count key " + _countKey);
        if (_countKey >= keyNumbers)
        {
            isInteractable = true;
            platform[0].SetActive(true);
        }
        else
        {
            isInteractable = false;
        }
    }
}