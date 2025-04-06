using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundInterface : MonoBehaviour
{
	public Rigidbody2D rb;
	public float footstepVolumeTopSpeed;
	public float footstepMaxVolume;
	public float footstepMaxVariance;
	public float footstepMinVolume;
	public float lightstepVolume;
	public float stillJumpMaxVariance;
	public float speedWindMultiplier;
	float currentSpeedWindVolume;
	public float minimumWindSpeedMagnitude;
	public float speedWindLerp;
	float footstepBasePitch1;
	float footstepBasePitch2;
	float footstepStartBasePitch;
	float stillJumpBasePitch;
	
	public AudioSource background;
	
    public AudioSource step1;
    public AudioSource step2;
	public AudioSource runningJump;
	public AudioSource stillJump;
	public AudioSource stillJumpLand;
	public AudioSource runningJumpLand;
	public AudioSource cornerGrab;
	public AudioSource cornerClimb;
	public AudioSource wallImpact;
	public AudioSource wallJump;
	public AudioSource footScuff;
	public AudioSource speedWind;

	public AudioClip footScuffClip;
	
	void Start(){
		footstepBasePitch1 = step1.pitch;
		footstepBasePitch2 = step2.pitch;
		footstepStartBasePitch = footScuff.pitch;
		stillJumpBasePitch = stillJump.pitch;
		/*
		speedWind.loop = true;
		speedWind.volume = 0.0f;
		currentSpeedWindVolume = 0.0f;
		speedWind.Play();
		*/
	}

	void Update(){
		/*
		//currentSpeedWindVolume = Mathf.Lerp(currentSpeedWindVolume, Mathf.Clamp(-rb.velocity.y - minimumWindSpeedMagnitude, 0.0f, 10.0f) * speedWindMultiplier, Time.deltaTime * speedWindLerp);
		//speedWind.volume = currentSpeedWindVolume;
		*/
	}
	
	public void PrintEvent(){
		Debug.Log("STEP");
	}
	
	public void PlayStep1(){
		step1.volume = Mathf.Clamp(Mathf.Abs(rb.velocity.x / footstepVolumeTopSpeed) * footstepMaxVolume, footstepMinVolume, 10f);
		step1.pitch = footstepBasePitch1 + (Random.Range(-1.0f, 1.0f) * footstepMaxVariance);
		step1.Play();
	}
	
	public void PlayStep2(){
		step2.volume = Mathf.Clamp(Mathf.Abs(rb.velocity.x / footstepVolumeTopSpeed) * footstepMaxVolume, footstepMinVolume, 10f);
		step2.pitch = footstepBasePitch2 + (Random.Range(-1.0f, 1.0f) * footstepMaxVariance);
		step2.Play();
	}

	public void PlayLightStep1(){
		step1.volume = lightstepVolume;
		step1.pitch = footstepBasePitch1 + (Random.Range(-1.0f, 1.0f) * footstepMaxVariance);
		step1.Play();
	}

	public void PlayLightStep2(){
		step2.volume = lightstepVolume;
		step2.pitch = footstepBasePitch2 + (Random.Range(-1.0f, 1.0f) * footstepMaxVariance);
		step2.Play();
	}
	
	public void PlayStepStart(){
		footScuff.pitch = footstepBasePitch1 + (Random.Range(-1.0f, 1.0f) * footstepMaxVariance);
		footScuff.Play();
	}
	
	public void PlayRunningJump(){
		runningJump.volume = Mathf.Abs(rb.velocity.x / footstepVolumeTopSpeed) * footstepMaxVolume;
		runningJump.Play();
	}
	
	public void PlayWallJump(){
		footScuff.pitch = footstepStartBasePitch + (Random.Range(-1.0f, 1.0f) * footstepMaxVariance);
		footScuff.PlayOneShot(footScuffClip);
	}
	
	public void PlayStillJumpLand(){
		stillJumpLand.Play();
	}
	
	public void PlayRunningJumpLand(){
		//runningJumpLand.Play();
	}
	
	public void PlayWallImpact(){
		//wallImpact.Play();
	}
	
	public void PlayCornerGrab(){
		//cornerGrab.Play();
	}
	
	public void PlayCornerClimb(){
		//cornerClimb.Play();
	}
	
	public void PlayStillJump(){
		stillJump.pitch = stillJumpBasePitch + (Random.Range(-1.0f, 1.0f) * stillJumpMaxVariance);
		stillJump.Play();
	}
}
