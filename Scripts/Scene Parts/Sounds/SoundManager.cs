using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public GameObject backgroundSoundManagerPrefab;
    public GameObject musicManagerPrefab;
    public AudioClip bgAudioClip;
    public float bgAudioVolume = 1.0f;
    public float bgAudioPitch = 1.0f;
    BackgroundSoundManager bgSoundManager;
    // Start is called before the first frame update
    void Start()
    {
        GameObject bgSoundManagerTest = GameObject.FindWithTag("BackgroundSoundManager");
        if(bgSoundManagerTest == null){
            GameObject s = Instantiate(backgroundSoundManagerPrefab,new Vector3(0,0,0),Quaternion.identity);
            bgSoundManager = s.GetComponent<BackgroundSoundManager>();
        } else{
            bgSoundManager = bgSoundManagerTest.GetComponent<BackgroundSoundManager>();
        }
        bgSoundManager.PlayBackgroundSound(bgAudioClip,bgAudioVolume,bgAudioPitch);
        bgSoundManager.SceneTransitionFadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
