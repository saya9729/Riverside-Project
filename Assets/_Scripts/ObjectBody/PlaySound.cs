using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    enum SoundName {slash, stab, shoot, reload, secondJump, collect, timeskill, walk, jump, dash, enemyHit, HUD, UI, menu, ambience, playerHit, slide}
    SoundName soundName;


    void playsound(SoundName soundName)
    {
        AudioInterface.PlayAudio(soundName.ToString());
    }
}
