using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimingManager : MonoBehaviour
{
	public TextMeshProUGUI nameText;
	float time;
	
	void Start(){
		time = 0.0f;
	}
	
	void Update(){
		time += Time.deltaTime;
		nameText.text = time.ToString();
	}
	
	public void StartTimer(){
		
	}
}
