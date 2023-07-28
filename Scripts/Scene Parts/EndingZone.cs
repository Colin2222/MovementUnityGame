using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingZone : MonoBehaviour
{
	public TimingManager timingManager;
	public GameObject leaderboardObject;
	public Leaderboard leaderboard;
    void OnTriggerEnter2D(Collider2D col){
		timingManager.FinishLevelTimer();
		leaderboardObject.SetActive(true);
		leaderboard.ActivateLeaderboard();
	}
}
