using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BackgroundSoundManager : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioSource bgAudio;
    bool isFadingOut = false;
    bool isFadingIn = false;
    public float fadeOutTime = 1.0f;
    public float fadeInTime = 1.0f;
    float fadeOutTimer = 0.0f;
    float fadeInTimer = 0.0f;

    float backgroundVolume = 0.0f;
    float musicVolume = 0.0f;
    float sfxVolume = 0.0f;

    public MusicManager musicManager;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        //musicManager.PlaySong();
    }

    // Update is called once per frame
    void Update()
    {
        if(isFadingOut)
        {
            fadeOutTimer -= Time.deltaTime;
            if (fadeOutTimer <= 0.0f){
                isFadingOut = false;
            }
            backgroundVolume = -80.0f + (80.0f * fadeOutTimer / fadeOutTime);
            mixer.SetFloat("VolumeBackground", backgroundVolume);

            sfxVolume = -80.0f + (80.0f * fadeOutTimer / fadeOutTime);
            mixer.SetFloat("VolumeSFX", sfxVolume);
        }

        if(isFadingIn)
        {
            fadeInTimer += Time.deltaTime;
            if (fadeInTimer >= fadeInTime){
                isFadingIn = false;
            }
            backgroundVolume = -80.0f + (80.0f * Mathf.Clamp01(fadeInTimer / fadeInTime));
            mixer.SetFloat("VolumeBackground", backgroundVolume);

            sfxVolume = -80.0f + (80.0f * Mathf.Clamp01(fadeInTimer / fadeInTime));
            mixer.SetFloat("VolumeSFX", sfxVolume);
        }
    }

    public void PlayBackgroundSound(AudioClip clip, float volume, float pitch)
    {
        bgAudio.clip = clip;
        bgAudio.volume = volume;
        bgAudio.pitch = pitch;
        bgAudio.loop = true;
        bgAudio.Play();
    }

    public void SceneTransitionFadeOut()
    {
        isFadingOut = true;
        fadeOutTime = fadeOutTime;
        fadeOutTimer = fadeOutTime;
    }

    public void SceneTransitionFadeIn()
    {
        isFadingOut = false;
        isFadingIn = true;
        fadeInTime = fadeInTime;
        fadeInTimer = 0.0f;
    }
}
