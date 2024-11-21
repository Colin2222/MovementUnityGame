using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
	Transform interactable;
	
    public Interactable Interact(){
		if(interactable != null){
			Interactable iFace = interactable.GetComponent<Interactable>();
			iFace.Interact();
			return iFace;
		}
		return null;
	}
	
	void OnTriggerEnter2D(Collider2D col){
		interactable = col.gameObject.transform;
	}
	
	void OnTriggerExit2D(Collider2D col){
		interactable = null;
	}
}
