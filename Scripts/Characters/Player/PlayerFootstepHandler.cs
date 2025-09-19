using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepHandler : MonoBehaviour
{
    public Transform checkPoint;
    public float checkDistance = 0.1f;
    public AudioSource[] footstepAudioSources;
    public AudioSource[] footScuffAudioSources;
    int activeAudioSource = 0;
    int activeScuffAudioSource = 0;
    public List<FootstepSoundMapping> footstepSoundListEditor;
    Dictionary<string, AudioClip> footstepSounds;
    LayerMask surfaceModifierLayer;
    public Rigidbody2D rb;
    public float footstepBasePitch;
    public float footstepMaxVariance;
    public float footstepVolumeTopSpeed;
    public float footstepMaxVolume;
    public float footstepMinVolume;
    float currentFootstepVolume;

    // Start is called before the first frame update
    void Start()
    {
        surfaceModifierLayer = LayerMask.GetMask("SurfaceModifier");

        footstepSounds = new Dictionary<string, AudioClip>();
        foreach (var mapping in footstepSoundListEditor)
        {
            footstepSounds[mapping.surfaceType] = mapping.footstepSound;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayFootstepSound(int side)
    {
        string surface_type = "stone";
        RaycastHit2D raycastHit = Physics2D.Raycast(checkPoint.position, Vector2.down, checkDistance, surfaceModifierLayer);
        if (raycastHit.collider != null)
        {
            SurfaceModifier sm = raycastHit.collider.GetComponent<SurfaceModifier>();
            if (sm != null)
            {
                surface_type = sm.surfaceType;
            }
        }

        AudioSource footstepAudioSource = footstepAudioSources[activeAudioSource];
        footstepAudioSource.clip = footstepSounds[surface_type];
        footstepAudioSource.panStereo = side * 0.2f;
        currentFootstepVolume = Mathf.Clamp(Mathf.Abs(rb.velocity.x / footstepVolumeTopSpeed) * footstepMaxVolume, footstepMinVolume, 10f);
        footstepAudioSource.volume = currentFootstepVolume;
        footstepAudioSource.pitch = footstepBasePitch + (Random.Range(-1.0f, 1.0f) * footstepMaxVariance);
        footstepAudioSource.Play();
    }

    public void PlayFootScuffSound(int side)
    {
        AudioSource footScuffAudioSource = footScuffAudioSources[activeScuffAudioSource];
        activeScuffAudioSource = (activeScuffAudioSource + 1) % footScuffAudioSources.Length;

        footScuffAudioSource.panStereo = side * 0.2f;
        footScuffAudioSource.volume = 0.1f - (currentFootstepVolume * 0.65f);
        footScuffAudioSource.pitch = footstepBasePitch + 0.2f + (Random.Range(-1.0f, 1.0f) * footstepMaxVariance);
        footScuffAudioSource.Play();
    }
}

[System.Serializable]
public class FootstepSoundMapping
{
    public string surfaceType;
    public AudioClip footstepSound;
}
