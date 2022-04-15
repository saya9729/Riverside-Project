using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AbstractClass;
using HighlightPlus;

public class KeyInteract : Interactable
{
    private HighlightEffect _highlightEffect;
    

    private void Start()
    {
        _highlightEffect = GetComponent<HighlightEffect>();

        _interactable = true;
        _highlightEffect.SetHighlighted(true);        
    }

    public override string GetDescription()
    {
        if (_interactable)
        {
            return "[E] Put out the fire";
        }
        else
        {
            return "";
        }        
    }
    public override void Interact()
    {
        _highlightEffect.SetHighlighted(false);
        _interactable = false;
    }


}
