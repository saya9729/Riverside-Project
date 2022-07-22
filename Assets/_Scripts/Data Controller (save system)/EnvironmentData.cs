[System.Serializable]
public class EnvironmentData
{
    public bool[] isKeyInteractableArray;

    public EnvironmentData(Player.KeyInteract[] p_keyInteract)
    {
        isKeyInteractableArray = new bool[p_keyInteract.Length];
        for (int i = 0; i < p_keyInteract.Length; i++)
        {
            isKeyInteractableArray[i] = p_keyInteract[i].IsInteractable();
        }
    }
}
