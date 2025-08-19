using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSoundInterface : MonoBehaviour
{
    public NPCSoundPlayer soundPlayer; 

    public void PlaySound(string soundName)
    {
        soundPlayer.PlaySound(soundName);
    }
}
