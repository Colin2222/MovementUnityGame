using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainAudioScript : MonoBehaviour
{
    public AudioSource rainAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        rainAudioSource.loop = true;
        rainAudioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
