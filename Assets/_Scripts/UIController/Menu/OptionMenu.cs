using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace GameUI
{
    public class OptionMenu : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private TMPro.TMP_Dropdown resolutionDropdown;
        [SerializeField] private Toggle toggleFullScreen;
        [SerializeField] private Slider sliderMusic;

        private Resolution[] _resolutions;
        private void Start()
        {
            _resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();
            int currentResolutionIndex = 0;
            List<string> resolutionOptions = new List<string>();
            for (int i = 0; i < _resolutions.Length; i++)
            {
                resolutionOptions.Add(_resolutions[i].width + " x " + _resolutions[i].height);
                if (_resolutions[i].width == Screen.width &&
                    _resolutions[i].height == Screen.height)
                {
                    currentResolutionIndex = i;
                }
            }
            resolutionDropdown.AddOptions(resolutionOptions);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
            toggleFullScreen.isOn = Screen.fullScreen;
            float MasterVolumeCurrentValue = PlayerPrefs.GetFloat("MasterVolume", 1f);

            //Set the music volume to the saved volume
            sliderMusic.value = MasterVolumeCurrentValue;
        }
        public void SetVolume(float p_volume)
        {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(p_volume) * 20);
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
}
