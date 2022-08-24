using System;
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
        [SerializeField] private Slider sliderMaster;
        [SerializeField] private Slider sliderMusic;
        [SerializeField] private Slider sliderEffects;
        [SerializeField] private Toggle HUD;

        private Resolution[] _resolutions;
        private void Start()
        {
            _resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();
            int currentResolutionIndex = 0;
            List<string> resolutionOptions = new List<string>();
            for (int i = 0; i < _resolutions.Length; i++)
            {
                
                resolutionOptions.Add(_resolutions[i].width + " x " + _resolutions[i].height + ", " + _resolutions[i].refreshRate + " HZ");
                if (_resolutions[i].width == Screen.width &&
                    _resolutions[i].height == Screen.height
                    && _resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
                {
                    currentResolutionIndex = i;
                }
            }
            resolutionDropdown.AddOptions(resolutionOptions);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();

            toggleFullScreen.isOn = Screen.fullScreen;

            float MasterVolumeCurrentValue = PlayerPrefs.GetFloat(PlayerPrefEnum.MasterVolume.ToString(), 1f);
            float MusicVolumeCurrentValue = PlayerPrefs.GetFloat(PlayerPrefEnum.MusicVolume.ToString(), 1f);
            float EffectsVolumeCurrentValue = PlayerPrefs.GetFloat(PlayerPrefEnum.EffectsVolume.ToString(), 1f);
            
            //Set the music volume to the saved volume
            sliderMaster.value = MasterVolumeCurrentValue;
            sliderMusic.value = MusicVolumeCurrentValue;
            sliderEffects.value = EffectsVolumeCurrentValue;

            HUD.isOn = Convert.ToBoolean(PlayerPrefs.GetInt(PlayerPrefEnum.HUD.ToString(), 1));
        }
        public void SetMasterVolume(float p_volume)
        {
            audioMixer.SetFloat(PlayerPrefEnum.MasterVolume.ToString(), Mathf.Log10(p_volume) * 20);
            PlayerPrefs.SetFloat(PlayerPrefEnum.MasterVolume.ToString(), p_volume);
            PlayerPrefs.Save();
        }

        public void SetMusicVolume(float p_volume)
        {
            audioMixer.SetFloat(PlayerPrefEnum.MusicVolume.ToString(), Mathf.Log10(p_volume) * 20);
            PlayerPrefs.SetFloat(PlayerPrefEnum.MusicVolume.ToString(), p_volume);
            PlayerPrefs.Save();
        }

        public void SetEffectsVolume(float p_volume)
        {
            audioMixer.SetFloat(PlayerPrefEnum.EffectsVolume.ToString(), Mathf.Log10(p_volume) * 20);
            PlayerPrefs.SetFloat(PlayerPrefEnum.EffectsVolume.ToString(), p_volume);
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

        public void SetHUD(bool p_isHUD)
        {
            PlayerPrefs.SetInt(PlayerPrefEnum.HUD.ToString(), p_isHUD ? 1 : 0);
            PlayerPrefs.Save();
            this.PostEvent(EventID.onToggleUI, p_isHUD);
        }
    }
}
