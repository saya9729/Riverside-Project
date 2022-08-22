[System.Serializable]
public enum PlayerPrefEnum
{
    None = 0,
    CurrentScene,

    //if you rename volume remember to rename their mixerGroup to the same in the AudioMixer
    MasterVolume,
    EffectsVolume,
    MusicVolume,


    HUD
}