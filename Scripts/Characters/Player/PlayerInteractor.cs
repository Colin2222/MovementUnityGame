using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
	Transform interactable;
	
    public void Interact(){
		if(interactable != null){
			IInteractable iFace = interactable.GetComponent<IInteractable>();
			iFace.Interact();
		}
	}
	
	void OnTriggerEnter2D(Collider2D col){
		interactable = col.gameObject.transform;
	}
	
	void OnTriggerExit2D(Collider2D col){
		interactable = null;
	}
}
