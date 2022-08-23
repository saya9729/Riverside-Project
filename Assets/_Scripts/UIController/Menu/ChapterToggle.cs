using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ChapterToggle : MonoBehaviour
{
    [SerializeField] private int chapter;
    [SerializeField] private GameObject disableState;
    [SerializeField] private GameObject enableState;

    private Button _chapterButton;
    private void Start()
    {
        _chapterButton = GetComponent<Button>();
        ToggleChapter();
    }
    public void ToggleChapter()
    {
        if (chapter == 1)
        {
            ToggleChaperState(true);
        }
        else
        {
            ToggleChaperState(SaveManager.FileSavePlayerExist(chapter));
        }
    }
    public void ToggleChaperState(bool p_state)
    {
        _chapterButton.interactable = p_state;
        ToggleChapterEffect(_chapterButton.interactable);
    }

    public void ToggleChapterEffect(bool p_state)
    {
        if (enableState)
        {
            enableState.SetActive(p_state);
        }
        if (disableState)
        {
            disableState.SetActive(!p_state);
        }
    }
}
