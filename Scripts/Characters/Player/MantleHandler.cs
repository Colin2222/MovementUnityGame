using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantleHandler : MonoBehaviour
{
	public CornerHandler cornerHandler;
	
    void OnTriggerEnter2D(Collider2D col){
		cornerHandler.mantleCorner = col.gameObject.transform;
		Debug.Log("MANTLE CORNER");
	}
	
	void OnTriggerExit2D(Collider2D col){
		cornerHandler.lastCorner = cornerHandler.mantleCorner;
		cornerHandler.mantleCorner = null;
	}
}
