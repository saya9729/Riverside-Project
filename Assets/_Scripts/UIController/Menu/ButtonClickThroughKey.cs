using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// This just only cover escape key
[RequireComponent(typeof(Button))]
public class ButtonClickThroughKey : MonoBehaviour
{
    private Button _button;
    private void Start()
    {
        _button = GetComponent<Button>();
    }
    public void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            _button.onClick.Invoke();
        }
    }
}
