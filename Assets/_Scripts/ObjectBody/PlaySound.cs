using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    void Playsound(AudioID soundName)
    {
        this.PostEvent(EventID.onPlaySound, soundName);
    }
}
