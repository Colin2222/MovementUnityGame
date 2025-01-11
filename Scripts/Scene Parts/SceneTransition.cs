using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
	public int transitionBuildIndex;
	public int transitionDirection;
	public int entranceNumber;
	public Transform spawnTransform;
	SessionManager sessionManager;
	
	void Awake(){
		GameObject sessionManagerTest = GameObject.FindWithTag("SessionManager");
		if(sessionManagerTest != null){
			sessionManager = sessionManagerTest.GetComponent<SessionManager>();
		}
	}
	
    void OnTriggerEnter2D(Collider2D col){
		sessionManager.TransitionScene(transitionBuildIndex, entranceNumber, transitionDirection);
	}
}
