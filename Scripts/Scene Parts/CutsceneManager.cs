using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
	float cutsceneTimer;
	float cutsceneDuration;
	float nextEventTime;
	bool inCutscene;
	
	public SceneManager sceneManager;
	
	void Awake(){
		inCutscene = false;
	}
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(inCutscene){
			cutsceneTimer += Time.deltaTime;
			
			if(cutsceneTimer >= cutsceneDuration){
				sceneManager.player.UnlockPlayer();
				inCutscene = false;
			}
		}
    }
	
	public void StartCutscene(string cutsceneName, float cutsceneDuration){
		cutsceneTimer = 0.0f;
		this.cutsceneDuration = cutsceneDuration;
		inCutscene = true; 
		sceneManager.player.LockPlayer();
	}
}
