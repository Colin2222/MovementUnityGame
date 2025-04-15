using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizerInteractable : Interactable
{
	CustomizerManager customizer;
	
    // Start is called before the first frame update
    void Start()
    {
        customizer = GameObject.FindWithTag("CustomizerManager").GetComponent<CustomizerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public override void Interact(){
		customizer.ActivatePreviewer();
	}

    public override void LeaveInteraction(){
        
    }

    public override void MenuUp()
    {
        
    }

    public override void MenuDown()
    {
        
    }

    public override void MenuLeft()
    {
        
    }

    public override void MenuRight()
    {
        
    }

    public override void MenuSelect()
    {
        
    }

    public override void MenuInteract()
    {
        
    }
}
