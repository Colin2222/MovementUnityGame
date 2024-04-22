using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerTracker : MonoBehaviour
{
    public CornerHandler cornerHandler;
	int numCorners = 0;
	
    void OnTriggerEnter2D(Collider2D col){
		cornerHandler.trackedCorner = col.gameObject.transform;
		numCorners++;
	}
	
	void OnTriggerExit2D(Collider2D col){
		cornerHandler.lastTrackedCorner = cornerHandler.mantleCorner;
		numCorners--;
		if(numCorners == 0){
			cornerHandler.trackedCorner = null;
		}
	}
}
