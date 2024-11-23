using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Site : Interactable
{
    public int id;
    public int type;
    public float cameraDistance;
    public Vector2 cameraOffset;
    public bool hasMenu;
    public bool hasPlayerInventoryMenu;

    public override void Interact(){
        Debug.Log("Interacting with site");
    }

    public override void LeaveInteraction(){
        Debug.Log("Leaving interaction with site");
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.tag == "PlayerInteractor"){
            EnterRange();
        }
	}
	
	void OnTriggerExit2D(Collider2D col){
        if(col.tag == "PlayerInteractor"){
            ExitRange();
        }
	}

    abstract protected void EnterRange();
    abstract protected void ExitRange();
}
