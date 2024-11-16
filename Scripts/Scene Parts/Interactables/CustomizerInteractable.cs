using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizerInteractable : MonoBehaviour, IInteractable
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
	
	public void Interact(){
		customizer.ActivatePreviewer();
	}

    public void LeaveInteraction(){
        
    }
}
