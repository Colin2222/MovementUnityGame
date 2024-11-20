using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTransitionInteractable : IInteractable
{
	public int buildIndex;
	public int entranceNumber;
	public int entranceDirection;
	
    public override void Interact(){
		GameObject.FindWithTag("Player").GetComponent<Animator>().Play("PlayerEnteringDoor");
		StartCoroutine(GameObject.FindWithTag("SceneManager").GetComponent<SceneManager>().SwitchScenes(buildIndex));
	}

	public override void LeaveInteraction(){
        
    }
}
