using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerHandler : MonoBehaviour
{
	[System.NonSerialized]
	public Transform corner = null;
	[System.NonSerialized]
	public Transform lastCorner = null;
	
	public float cornerOffsetX;
	public float cornerOffsetY;
	public float cornerClimbOffsetX;
	public float cornerClimbOffsetY;
	public float cornerEndClimbOffsetX;
	public float cornerEndClimbOffsetY;
	
    void OnTriggerEnter2D(Collider2D col){
		corner = col.gameObject.transform;
	}
	
	void OnTriggerExit2D(Collider2D col){
		lastCorner = corner;
		corner = null;
	}
}
