using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    enum SoundName {shot, punch}
    SoundName soundName;


    void playsound(SoundName soundName)
    {
        FindObjectOfType<AudioManager>().Play(soundName.ToString());
    }
}
