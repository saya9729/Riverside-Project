using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer AudioMixer;
    [SerializeField] private TMPro.TMP_Dropdown ResolutionDropdown;
    [SerializeField] private Toggle ToggleFullScreen;
    [SerializeField] private Slider SliderMusic;

    private Resolution[] _resolutions;
    private void Start()
    {
        _resolutions = Screen.resolutions;
        ResolutionDropdown.ClearOptions();
        int currentResolutionIndex = 0;
        List<string> resolutionOptions = new List<string>();
        for(int i =0; i<_resolutions.Length; i++)
        {
            resolutionOptions.Add(_resolutions[i].width + " x " + _resolutions[i].height);
            if (_resolutions[i].width == Screen.width &&
                _resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        ResolutionDropdown.AddOptions(resolutionOptions);
        ResolutionDropdown.value = currentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();
        ToggleFullScreen.isOn = Screen.fullScreen;
        float MasterVolumeCurrentValue = PlayerPrefs.GetFloat("MasterVolume", 0f);

        //Set the music volume to the saved volume
        SliderMusic.value = MasterVolumeCurrentValue;
    }
    public void SetVolume(float p_volume)
    {
        AudioMixer.SetFloat("MasterVolume", p_volume);
        PlayerPrefs.SetFloat("MasterVolume", p_volume);
        PlayerPrefs.Save();
    }
    public void SetFullScreen(bool p_isFullScreen)
    {
        Screen.fullScreen = p_isFullScreen;
    }
    public void SetResolution(int p_resolutionIndex)
    {
        Resolution resolution = _resolutions[p_resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
    }
}
