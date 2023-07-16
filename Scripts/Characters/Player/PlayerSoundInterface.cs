using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundInterface : MonoBehaviour
{
    public AudioSource step1;
    public AudioSource step2;
	
	public void PrintEvent(){
		Debug.Log("STEP");
	}
	
	public void PlayStep1(){
		step1.Play();
	}
	
	public void PlayStep2(){
		step2.Play();
	}
}
