using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioInterface
{
    public static void PlayAudio(string audioName)
    {
        if (!AudioManager.instance)
        {
            Debug.LogWarning("Missing AudioManager!");
            return;
        }
        if (!AudioManager.instance.GetSound(audioName).source.isPlaying)
        {
            AudioManager.instance.Play(audioName);
        }
    }

    public static void StopAudio(string audioName)
    {
        if (!AudioManager.instance)
        {
            Debug.LogWarning("Missing audio manager!");
            return;
        }
        AudioManager.instance.Stop(audioName);
    }
}