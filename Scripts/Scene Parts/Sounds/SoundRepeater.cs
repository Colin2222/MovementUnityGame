using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRepeater : MonoBehaviour
{
    public AudioSource audioSource;
    public float repeatTime;
    public float startTime;

    public bool loopingOnStart;

    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        audioSource.time = startTime;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(!loopingOnStart){
            timer += Time.deltaTime;
            if(timer >= repeatTime){
                audioSource.time = startTime;
                audioSource.Play();
                timer = 0;
            }
        }
    }
}
