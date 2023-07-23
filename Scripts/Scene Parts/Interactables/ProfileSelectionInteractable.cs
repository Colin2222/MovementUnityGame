using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileSelectionInteractable : MonoBehaviour, IInteractable
{
	[System.NonSerialized]
	public ProfileManager profileManager;
	[System.NonSerialized]
	public PlayerProfile profile;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void Interact(){
		profileManager.SetProfileText(profile.displayName);
	}
}
