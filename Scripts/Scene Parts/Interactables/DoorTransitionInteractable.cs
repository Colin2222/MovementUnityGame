using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTransitionInteractable : Interactable
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

	public override void MenuUp(){
		
	}

	public override void MenuDown(){
		
	}

	public override void MenuLeft(){
		
	}

	public override void MenuRight(){
		
	}

	public override void MenuSelect(){
		
	}
}
