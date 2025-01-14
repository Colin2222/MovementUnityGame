using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Site : Interactable
{
    [System.NonSerialized]
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
    }

    abstract protected void EnterRange();
    abstract protected void ExitRange();
    abstract public void LoadSite(SavedSite savedSite);
    abstract public SavedSite SaveSite();
    abstract public void ConstructSite();

    public void ActivateCamera(){
        // set camera to split player and interactable
        Vector3 midPoint = (SceneManager.Instance.player.transform.position + gameObject.transform.position) / 2;
        GameObject cameraTargetObj = new GameObject();
        cameraTargetObj.transform.position = midPoint + new Vector3(cameraOffset.x, cameraOffset.y, 0);
        SceneManager.Instance.SetCamera(cameraTargetObj, cameraDistance);
    }

    public void DeactivateCamera(){
        SceneManager.Instance.ResetCamera();
    }
}
