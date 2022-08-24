using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup effectsMixerGroup;

    public Sound[] sounds;

    void Start()
    {
        this.RegisterListener(EventID.onPlaySound, (param) => PlaySound((AudioID)param));
        this.RegisterListener(EventID.onStopSound, (param) => StopSound((AudioID)param));

        PlaySceneAudio();
    }

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;

            switch (s.audioType)
            {
                case Sound.AudioType.Effect:
                    s.source.outputAudioMixerGroup = effectsMixerGroup;
                    break;
                case Sound.AudioType.Music:
                    s.source.outputAudioMixerGroup = musicMixerGroup;
                    break;
            }
        }
    }

    public void PlaySound(AudioID audioName)
    {
        Sound s = Array.Find(sounds, item => item.name == audioName.ToString());
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Play();
    }

    public void StopSound(AudioID audioName)
    {
        Sound s = Array.Find(sounds, item => item.name == audioName.ToString());
        if (s.source.isPlaying)
        {
            s.source.Stop();
        }
    }

    public void PlaySoundUI(string audioName)
    {
        Sound s = Array.Find(sounds, item => item.name == audioName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Play();
    }

    public void PlaySceneAudio()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlaySound(AudioID.backgroundmusic);
        }
        else 
        {
            PlaySound(AudioID.ambience);
            PlaySound(AudioID.subAmbience);
        }
    }
}
