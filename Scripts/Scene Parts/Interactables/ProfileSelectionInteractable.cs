using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileSelectionInteractable : Interactable
{
	[System.NonSerialized]
	public ProfileManager profileManager;
	[System.NonSerialized]
	public int profileId;
	public ProfileSelectionPreviewer previewer;
	
	public float transitionTime;
	float transitionTimer;
	bool transitioning = false;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if(transitioning){
			transitionTimer -= Time.deltaTime;
			if(transitionTimer <= 0.0f){
				transitioning = false; 
				profileManager.SetCurrentProfile(profileId);
			}
		}
    }
	
	public override void Interact(){
		transitionTimer = transitionTime;
		transitioning = true;
		GameObject.FindWithTag("SceneTransitionManager").GetComponent<SceneTransitionManager>().TempTransition(transitionTime);
	}

	public override void LeaveInteraction(){
        
    }
	
	public void SetCustomization(List<(int, int)> customizationData, CustomizerColorPalette colorManager){
		previewer.SyncCustomizationData(customizationData, colorManager);
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
