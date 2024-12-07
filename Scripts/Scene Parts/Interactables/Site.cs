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
    public Dictionary<string, Inventory> inventories;
    protected string configPath;

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

    void Start(){
        inventories = new Dictionary<string, Inventory>();

        // read the json file

    }

    abstract protected void EnterRange();
    abstract protected void ExitRange();
    abstract public void MenuUp();
    abstract public void MenuDown();
    abstract public void MenuLeft();
    abstract public void MenuRight();
    abstract public void MenuSelect();
    abstract public void LoadSite(SavedSite savedSite);
}
