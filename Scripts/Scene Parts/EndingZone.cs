using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingZone : MonoBehaviour
{
	public TimingManager timingManager;
    void OnTriggerEnter2D(Collider2D col){
		timingManager.StopTimer();
	}
}
