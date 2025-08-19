using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSoundPlayer : MonoBehaviour
{
    public Sound[] soundList; // List of sounds to be played
    Dictionary<string, Sound> sounds = new Dictionary<string, Sound>();
    AudioSource[] audioSources;
    int currentAudioSourceIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        audioSources = GetComponents<AudioSource>();
        foreach (Sound sound in soundList)
        {
            if (!sounds.ContainsKey(sound.name))
            {
                sounds.Add(sound.name, sound);
            }
        }
    }

    public void PlaySound(string soundName)
    {
        Sound sound = sounds[soundName];
        if (sound != null)
        {
            AudioSource audioSource = audioSources[currentAudioSourceIndex];
            audioSource.clip = sound.clip;
            audioSource.volume = sound.volume + Random.Range(-sound.volumeVariance, sound.volumeVariance);
            audioSource.pitch = sound.pitch + Random.Range(-sound.pitchVariance, sound.pitchVariance);
            audioSource.Play();
            currentAudioSourceIndex = (currentAudioSourceIndex + 1) % audioSources.Length;
        }
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public float volume;
    public float pitch;
    public float volumeVariance;
    public float pitchVariance;
}
