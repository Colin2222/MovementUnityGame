using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundInterface : MonoBehaviour
{
	public Rigidbody2D rb;
	public float footstepVolumeTopSpeed;
	
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
	
	public void PrintEvent(){
		Debug.Log("STEP");
	}
	
	public void PlayStep1(){
		step1.volume = Mathf.Abs(rb.velocity.x / footstepVolumeTopSpeed);
		step1.Play();
	}
	
	public void PlayStep2(){
		step2.volume = Mathf.Abs(rb.velocity.x / footstepVolumeTopSpeed);
		step2.Play();
	}
	
	public void PlayRunningJump(){
		runningJump.Play();
	}
}
