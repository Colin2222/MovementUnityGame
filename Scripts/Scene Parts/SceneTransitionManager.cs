using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
	public SceneManager sceneManager;
	public RawImage transitionShape;
	public RectTransform exitPos;
	public float transitionSpeed;
	public float transitionTime;
	float transitionTimer;
	bool exiting = false;
	bool entering = false;
	int sceneTransitionIndex;
    // Update is called once per frame
    void Update()
    {
        if(exiting){
			transitionShape.transform.localPosition += (transitionSpeed * Time.deltaTime * Vector3.right);
			transitionTimer -= Time.deltaTime;
			if(transitionTimer <= 0.0f){
				sceneManager.SwitchScenes(sceneTransitionIndex);
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
}
