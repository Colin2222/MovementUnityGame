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
	public bool isUpDownTransition;
	SessionManager sessionManager;

	void Awake()
	{
		GameObject sessionManagerTest = GameObject.FindWithTag("SessionManager");
		if (sessionManagerTest != null)
		{
			sessionManager = sessionManagerTest.GetComponent<SessionManager>();
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		// if it's an up-down transition, contact does not lead to transition
		//player must move up into the transition
		if (isUpDownTransition)
		{
			return;
		}

		if (!PlayerHub.Instance.isSpawning && isWalkTransition)
		{
			PlayerHub.Instance.overrideManager.WalkToPoint(exitTransform.position.x);
		}
		sessionManager.TransitionScene(transitionBuildIndex, entranceNumber, transitionDirection);
	}

	public bool UpDownTransition()
	{
		return sessionManager.TransitionScene(transitionBuildIndex, entranceNumber, transitionDirection);
	}
}
