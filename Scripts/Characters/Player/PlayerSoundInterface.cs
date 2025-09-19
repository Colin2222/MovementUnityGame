using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundInterface : MonoBehaviour
{
	public Rigidbody2D rb;
	public PlayerFootstepHandler footstepHandler;
	public float stillJumpMaxVariance;
	public float speedWindMultiplier;
	float currentSpeedWindVolume;
	public float minimumWindSpeedMagnitude;
	public float speedWindLerp;
	float stillJumpBasePitch;
	public float cornerClimbBasePitch;
	
	public AudioSource background;
	
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
	public AudioSource clothesRustle;

	public AudioClip footScuffClip;
	public AudioSource axeChop;
	
	void Start(){
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
		footstepHandler.PlayFootstepSound(-1);
	}
	
	public void PlayStep2(){
		footstepHandler.PlayFootstepSound(1);
	}
	
	public void PlayScuff1(){
		footstepHandler.PlayFootScuffSound(-1);
	}

	public void PlayScuff2(){
		footstepHandler.PlayFootScuffSound(1);
	}

	public void PlayLightStep1()
	{
		footstepHandler.PlayFootstepSound(-1);
	}

	public void PlayLightStep2(){
		footstepHandler.PlayFootstepSound(1);
	}
	
	public void PlayRunningJump(){
		footstepHandler.PlayFootstepSound(1);
	}
	
	public void PlayWallJump(){
		footScuff.pitch = 0.9f + (Random.Range(-1.0f, 1.0f) * stillJumpMaxVariance);
		footScuff.PlayOneShot(footScuffClip);
	}
	
	public void PlayStillJumpLand(){
		stillJumpLand.Play();
		clothesRustle.pitch = 1.0f + (Random.Range(-1.0f, 1.0f) * 0.25f);
		clothesRustle.Play();
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
		clothesRustle.pitch = cornerClimbBasePitch + (Random.Range(-1.0f, 1.0f) * stillJumpMaxVariance);
		clothesRustle.Play();
	}
	
	public void PlayStillJump(){
		stillJump.pitch = stillJumpBasePitch + (Random.Range(-1.0f, 1.0f) * stillJumpMaxVariance);
		stillJump.Play();
	}

	public void PlayAxeChop(){
		axeChop.Play();
	}
}
