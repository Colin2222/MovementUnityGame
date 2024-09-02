using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundInterface : MonoBehaviour
{
	public Rigidbody2D rb;
	public float footstepVolumeTopSpeed;
	public float footstepMaxVolume;
	public float footstepMaxVariance;
	public float stillJumpMaxVariance;
	float footstepBasePitch1;
	float footstepBasePitch2;
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
	
	void Start(){
		footstepBasePitch1 = step1.pitch;
		footstepBasePitch2 = step2.pitch;
		stillJumpBasePitch = stillJump.pitch;
	}
	
	public void SetBackgroundAudio(AudioClip bgAudio){
		background.clip = bgAudio;
		background.loop = true;
		background.Play();
	}
	
	public void PrintEvent(){
		Debug.Log("STEP");
	}
	
	public void PlayStep1(){
		step1.volume = Mathf.Abs(rb.velocity.x / footstepVolumeTopSpeed) * footstepMaxVolume;
		step1.pitch = footstepBasePitch1 + (Random.Range(-1.0f, 1.0f) * footstepMaxVariance);
		step1.Play();
	}
	
	public void PlayStep2(){
		step2.volume = Mathf.Abs(rb.velocity.x / footstepVolumeTopSpeed) * footstepMaxVolume;
		step2.pitch = footstepBasePitch2 + (Random.Range(-1.0f, 1.0f) * footstepMaxVariance);
		step2.Play();
	}
	
	public void PlayRunningJump(){
		runningJump.volume = Mathf.Abs(rb.velocity.x / footstepVolumeTopSpeed) * footstepMaxVolume;
		runningJump.Play();
	}
	
	public void PlayWallJump(){
		//wallJump.Play();
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
