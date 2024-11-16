using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTransitionInteractable : MonoBehaviour, IInteractable
{
	public int buildIndex;
	public int entranceNumber;
	public int entranceDirection;
	
    public void Interact(){
		GameObject.FindWithTag("Player").GetComponent<Animator>().Play("PlayerEnteringDoor");
		StartCoroutine(GameObject.FindWithTag("SceneManager").GetComponent<SceneManager>().SwitchScenes(buildIndex));
	}

	public void LeaveInteraction(){
        
    }
}
