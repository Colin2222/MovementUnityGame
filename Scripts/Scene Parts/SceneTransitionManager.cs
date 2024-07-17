using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
	public SceneManager sceneManager;
	PersistentState persistentState;
	public RawImage transitionShape;
	public RectTransform exitPos;
	public float transitionSpeed;
	public float transitionTime;
	float transitionTimer;
	float timeUntilAction;
	bool exiting = false;
	bool entering = false;
	bool tempTransitioning = false;
	int sceneTransitionIndex;
	
	int currentEntranceNumber;
	int currentTransitionDirection;
    // Update is called once per frame
    void Update()
    {
		if(tempTransitioning){
			if(exiting){
				transitionShape.transform.localPosition += (transitionSpeed * Time.deltaTime * Vector3.right);
				transitionTimer -= Time.deltaTime;
				if(transitionTimer <= 0.0f){
					exiting = false;
					transitionTimer = timeUntilAction - transitionTime;
				}
			} else if(entering){
				transitionShape.transform.localPosition += (transitionSpeed * Time.deltaTime * Vector3.right);
				transitionTimer -= Time.deltaTime;
				if(transitionTimer <= 0.0f){
					entering = false;
					tempTransitioning = false;
					transitionShape.gameObject.SetActive(false);
				}
			} else{
				transitionTimer -= Time.deltaTime;
				if(transitionTimer <= 0.0f){
					EnterTransition();
				}
			}
		} else{
			if(exiting){
				transitionShape.transform.localPosition += (transitionSpeed * Time.deltaTime * Vector3.right);
				transitionTimer -= Time.deltaTime;
				if(transitionTimer <= 0.0f){
					transitionSpeed = 0.0f;
					//sceneManager.SwitchScenes(sceneTransitionIndex);
				}
			}
			
			if(entering){
				transitionShape.transform.localPosition += (transitionSpeed * Time.deltaTime * Vector3.right);
				transitionTimer -= Time.deltaTime;
				if(transitionTimer <= 0.0f){
					entering = false;
					transitionShape.gameObject.SetActive(false);
				}
			}
		}
    }
	
	public void ExitTransition(int buildIndex){
		transitionShape.gameObject.SetActive(true);
		sceneTransitionIndex = buildIndex;
		transitionShape.transform.localPosition = exitPos.transform.localPosition;
		transitionTimer = transitionTime;
		exiting = true;
	}
	
	public void EnterTransition(){
		transitionShape.gameObject.SetActive(true);
		transitionShape.transform.localPosition = Vector3.zero;
		transitionTimer = transitionTime;
		entering = true;
	}
	
	public void TempTransition(float timeUntilAction){
		this.timeUntilAction = timeUntilAction;
		transitionShape.gameObject.SetActive(true);
		transitionShape.transform.localPosition = exitPos.transform.localPosition;
		transitionTimer = transitionTime;
		exiting = true;
		tempTransitioning = true;
	}
}
