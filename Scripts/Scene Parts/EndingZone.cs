using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingZone : MonoBehaviour
{
	SceneManager sceneManager;
	PlayerHub player;
	public TimingManager timingManager;
	public GameObject leaderboardObject;
	public Leaderboard leaderboard;
	bool levelFinished = false; 
	
	void Start(){
		sceneManager = GameObject.FindWithTag("SceneManager").GetComponent<SceneManager>();
		player = GameObject.FindWithTag("Player").GetComponent<PlayerHub>();
	}
	
    void OnTriggerEnter2D(Collider2D col){
		levelFinished = true;
		timingManager.FinishLevelTimer();
		leaderboardObject.SetActive(true);
		leaderboard.ActivateLeaderboard();
		player.mover.LockPlayer();
	}
	
	void OnRestartLevel(){
		if(levelFinished){
			sceneManager.SwitchScenes(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
		}
	}
	
	void OnReturnToHub(){
		if(levelFinished){
			sceneManager.ReturnToHub();
		}
	}
}
