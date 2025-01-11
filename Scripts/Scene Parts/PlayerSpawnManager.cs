using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public SceneManager sceneManager;
    SessionManager sessionManager;

    public Transform playerSpawnTransform;

    PlayerHub player;
    
    void Awake(){
        player = sceneManager.player;
        sessionManager = sceneManager.sessionManager;
    }

    void Start()
    {
        // set player spawn point which has been determined by matching SceneTranstion to entranceNumber
		bool entranceFound = false;
		GameObject[] sceneTransitions = GameObject.FindGameObjectsWithTag("SceneTransition");
		foreach(GameObject transitionObj in sceneTransitions){
			SceneTransition transition = transitionObj.GetComponent<SceneTransition>();
			if(transition.entranceNumber == sessionManager.currentEntranceNumber){
				player.transform.position = transition.spawnTransform.position;
				entranceFound = true;
				break;
			}
		}
		if(!entranceFound){
			player.transform.position = playerSpawnTransform.position;
		}
		
		// clear the players tracking for its groundcheck since unity scene transition doesnt frickin exit a 2d collision dammit
		if(player.physics != null){
			player.physics.ClearBottomCheck();
		}
    }
}
