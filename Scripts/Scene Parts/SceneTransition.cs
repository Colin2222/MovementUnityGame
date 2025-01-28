using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
	public int transitionBuildIndex;
	public int transitionDirection;
	public int entranceNumber;
	public Transform spawnTransform;
	public Transform entryTransform;
	public Transform exitTransform;
	public bool isWalkTransition;
	SessionManager sessionManager;
	
	void Awake(){
		GameObject sessionManagerTest = GameObject.FindWithTag("SessionManager");
		if(sessionManagerTest != null){
			sessionManager = sessionManagerTest.GetComponent<SessionManager>();
		}
	}
	
    void OnTriggerEnter2D(Collider2D col){
		if(!PlayerHub.Instance.isSpawning && isWalkTransition){
			PlayerHub.Instance.overrideManager.WalkToPoint(exitTransform.position.x);
		}
		sessionManager.TransitionScene(transitionBuildIndex, entranceNumber, transitionDirection);
	}
}
