using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenCutsceneManager : MonoBehaviour
{
    public AudioSource axeChopAudioSource;
    public AudioSource metalRingAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAxeChop(){
        axeChopAudioSource.Play();
    }

    public void PlayMetalRing(){
        metalRingAudioSource.Play();
    }
}
