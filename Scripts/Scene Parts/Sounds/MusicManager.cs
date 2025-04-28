using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.AddressableAssets;

public class MusicManager : MonoBehaviour
{
    public AudioClip song;
    public AudioSource audioSource;

    public void PlaySong(string songName)
    {
        var operation = Addressables.LoadAssetAsync<AudioClip>("Assets/Sounds/Music/" + songName + ".wav");
		song = operation.WaitForCompletion();
        if (audioSource != null && song != null)
        {
            audioSource.clip = song;
            audioSource.Play();
        }
    }
}
