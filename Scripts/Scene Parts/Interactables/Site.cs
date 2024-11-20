using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Site : IInteractable
{
    public int id;
    public int type;
    public bool active;
    bool inConstruction;
    bool inRange;
    public GameObject rangeEffect;
    public Structure structure;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact(){
        Debug.Log("Interacting with site");
        active = true;
    }

    public override void LeaveInteraction(){
        Debug.Log("Leaving interaction with site");
        active = false;
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.tag == "PlayerInteractor"){
            inRange = true;
            rangeEffect.SetActive(true);
        }
	}
	
	void OnTriggerExit2D(Collider2D col){
		inRange = false;
        rangeEffect.SetActive(false);
	}
}
