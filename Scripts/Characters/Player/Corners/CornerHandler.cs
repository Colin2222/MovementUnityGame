using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerHandler : MonoBehaviour
{
	[System.NonSerialized]
	public Transform corner = null;
	[System.NonSerialized]
	public Transform lastCorner = null;
	[System.NonSerialized]
	public Transform mantleCorner = null;
	[System.NonSerialized]
	public Transform footCorner = null;
	
	[System.NonSerialized]
	public Transform trackedCorner = null;
	[System.NonSerialized]
	public Transform lastTrackedCorner = null;
	
	public float cornerOffsetX;
	public float cornerOffsetY;
	public float cornerClimbOffsetX;
	public float cornerClimbOffsetY;
	public float cornerEndClimbOffsetX;
	public float cornerEndClimbOffsetY;
	public float mantleClimbOffsetX;
	public float mantleClimbOffsetY;
	public float fastMantleEndOffsetX;
	public float fastMantleEndOffsetY;

	public FootHandler footHandler;
	
    void OnTriggerEnter2D(Collider2D col){
		corner = col.gameObject.transform;
	}
	
	void OnTriggerExit2D(Collider2D col){
		lastCorner = corner;
		corner = null;
	}
	
	public bool CheckFootHandler(int direction){
		return footHandler.CheckForCorner(direction);
	}
}
