using UnityEngine;

[System.Serializable]
public class EnvironmentData
{
    public bool[] keyInteractableArray;

    public EnvironmentData(Player.KeyInteract[] p_keyInteract)
    {
        keyInteractableArray = new bool[p_keyInteract.Length];
        for (int i = 0; i < p_keyInteract.Length; i++)
        {
            if (p_keyInteract[i].IsInteractable())
            {
                keyInteractableArray[i] = true;
            }
            else 
            { 
                keyInteractableArray[i] = false; 
            }
        }
    }
}
