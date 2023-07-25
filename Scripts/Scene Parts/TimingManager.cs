using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimingManager : MonoBehaviour
{
	[System.NonSerialized]
	public ProfileManager profileManager;
	
	public TextMeshProUGUI nameText;
	float time;
	bool isTiming = false;
	
	void Start(){
		time = 0.0f;
	}
	
	void Update(){
		if(isTiming){
			time += Time.deltaTime;
			nameText.text = time.ToString();
		}
	}
	
	public void StartTimer(){
		isTiming = true;
	}
	
	public void StopTimer(){
		isTiming = false;
	}
	
	public void FinishLevelTimer(){
		StopTimer();
		profileManager.RegisterNewTime(time);
	}
}
