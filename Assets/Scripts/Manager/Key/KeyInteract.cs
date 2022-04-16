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

        InteractableFlag = true;
        _highlightEffect.SetHighlighted(true);        
    }

    public override string GetDescription()
    {
        if (InteractableFlag)
        {
            return "[E] Interact";
        }
        else
        {
            return "";
        }        
    }
    public override void Interact()
    {
        _highlightEffect.SetHighlighted(false);
        InteractableFlag = false;
    }


}
