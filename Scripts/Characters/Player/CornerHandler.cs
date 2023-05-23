using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerHandler : MonoBehaviour
{
	[System.NonSerialized]
	public Transform corner = null;
	
	public float cornerOffsetX;
	public float cornerOffsetY;
	
    void OnTriggerEnter2D(Collider2D col){
		corner = col.gameObject.transform;
	}
	
	void OnTriggerExit2D(Collider2D col){
		corner = null;
	}
}
