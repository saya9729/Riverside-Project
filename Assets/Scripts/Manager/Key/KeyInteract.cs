using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AbstractClass;
using HighlightPlus;

public class KeyInteract : Interactable
{
    HighlightEffect _highlightEffect;

    private void Start()
    {
        _highlightEffect = GetComponent<HighlightEffect>();
    }

    public override string GetDescription()
    {
        return "[E] Light bonfire";
    }
    public override void Interact()
    {
        _highlightEffect.SetHighlighted(true);
    }


}
