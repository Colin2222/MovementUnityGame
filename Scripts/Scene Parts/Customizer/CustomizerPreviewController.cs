using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizerPreviewController : MonoBehaviour
{
	public CustomizerManager customizer;
	
	// 0 = HEADER
	// 1 = HEAD
	// 2 = BODY
	int currentPreviewer;
	
	public PartPreviewer headerPreviewer;
	public PartPreviewer headPreviewer;
	public PartPreviewer bodyPreviewer;
	public PartPreviewer legsPreviewer;
	public PartPreviewer armsPreviewer;
	
	List<(int, int)> customizerDataToSet;
	public float transitionTime;
	float transitionTimer;
	bool transitioning = false;
	
    // Start is called before the first frame update
    void Start()
    {
        currentPreviewer = 1;
		UpdatePreviewer();
    }
	
	void Update(){
		if(transitioning){
			transitionTimer -= Time.deltaTime;
			if(transitionTimer <= 0.0f){
				transitioning = false;
				
				customizer.StitchNewPlayerSpritesheet(customizerDataToSet);
				GameObject.FindWithTag("Player").GetComponent<PlayerHub>().UnlockPlayer();
				gameObject.SetActive(false);
			}
		}
	}
	
	public void InitializePreviewer(){
		ProfileManager profileManager = GameObject.FindWithTag("ProfileManager").GetComponent<ProfileManager>();
		if(profileManager.currentProfile != null){
			headerPreviewer.SetSelection(profileManager.currentProfile.customizationCodes[0]);
			headPreviewer.SetSelection(profileManager.currentProfile.customizationCodes[1]);
			bodyPreviewer.SetSelection(profileManager.currentProfile.customizationCodes[2]);
		}
		
		headerPreviewer.UpdateSelectionImage();
		headPreviewer.UpdateSelectionImage();
		bodyPreviewer.UpdateSelectionImage();
		legsPreviewer.UpdateSelectionImage();
		armsPreviewer.UpdateSelectionImage();
	}

	void UpdatePreviewer(){
		headerPreviewer.leftArrow.color = Color.white;
		headerPreviewer.rightArrow.color = Color.white;
		headPreviewer.leftArrow.color = Color.white;
		headPreviewer.rightArrow.color = Color.white;
		bodyPreviewer.leftArrow.color = Color.white;
		bodyPreviewer.rightArrow.color = Color.white;
		
		switch(currentPreviewer){
			case 0:
				headerPreviewer.leftArrow.color = Color.red;
				headerPreviewer.rightArrow.color = Color.red;
				break;
			case 1:
				headPreviewer.leftArrow.color = Color.red;
				headPreviewer.rightArrow.color = Color.red;
				break;
			case 2:
				bodyPreviewer.leftArrow.color = Color.red;
				bodyPreviewer.rightArrow.color = Color.red;
				break;
		}
	}
	
	void OnMoveRight(){
		switch(currentPreviewer){
			case 0:
				headerPreviewer.MoveSelectionRight();
				break;
			case 1:
				headPreviewer.MoveSelectionRight();
				break;
			case 2:
				bodyPreviewer.MoveSelectionRight();
				break;
		}
	}
	
	void OnMoveLeft(){
		switch(currentPreviewer){
			case 0:
				headerPreviewer.MoveSelectionLeft();
				break;
			case 1:
				headPreviewer.MoveSelectionLeft();
				break;
			case 2:
				bodyPreviewer.MoveSelectionLeft();
				break;
		}
	}
	
	void OnMoveUp(){
		switch(currentPreviewer){
			case 0:
				currentPreviewer = 2;
				break;
			case 1:
				currentPreviewer--;
				break;
			case 2:
				currentPreviewer--;
				break;
		}
		
		UpdatePreviewer();
	}
	
	void OnMoveDown(){
		switch(currentPreviewer){
			case 0:
				currentPreviewer++;
				break;
			case 1:
				currentPreviewer++;
				break;
			case 2:
				currentPreviewer = 0;
				break;
		}
		
		UpdatePreviewer();
	}
	
	void OnExit(){
		ProfileManager profileManager = GameObject.FindWithTag("ProfileManager").GetComponent<ProfileManager>();
		
		List<(int, int)> newCustomizerData = new List<(int, int)>();
		newCustomizerData.Add(headerPreviewer.GetSelection());
		newCustomizerData.Add(headPreviewer.GetSelection());
		newCustomizerData.Add(bodyPreviewer.GetSelection());
		if(profileManager.currentProfile != null){
			profileManager.SetCustomizationData(newCustomizerData);
		}
		customizerDataToSet = newCustomizerData;
		transitionTimer = transitionTime;
		transitioning = true;
		GameObject.FindWithTag("SceneTransitionManager").GetComponent<SceneTransitionManager>().TempTransition(transitionTime);
	}
	
	void OnColorRight(){
		switch(currentPreviewer){
			case 0:
				headerPreviewer.MoveColorRight();
				break;
			case 1:
				headPreviewer.MoveColorRight();
				break;
			case 2:
				bodyPreviewer.MoveColorRight();
				break;
		}
		
		UpdatePreviewer();
	}
	
	void OnColorLeft(){
		switch(currentPreviewer){
			case 0:
				headerPreviewer.MoveColorLeft();
				break;
			case 1:
				headPreviewer.MoveColorLeft();
				break;
			case 2:
				bodyPreviewer.MoveColorLeft();
				break;
		}
		
		UpdatePreviewer();
	}
}
