using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.AddressableAssets;
using Newtonsoft.Json;

public class CutsceneManager : MonoBehaviour
{
	float cutsceneTimer;
	float cutsceneDuration;
	string cutsceneAddressHeader = "Assets/Data/Cutscenes/";
	TextAsset currentCutsceneTxt;
	bool inCutscene;
	Cutscene currentCutscene;
	int currentTaskIndex;
	CutsceneTask currentTask;
	List<CutsceneAction> actions; 
	Dictionary<int, List<CutsceneActor>> actorsDict;
	Dictionary<int, Cutscene> cutsceneDict;
	
	public SceneManager sceneManager;
	
	void Awake(){
		inCutscene = false;
		actorsDict = new Dictionary<int, List<CutsceneActor>>();
		cutsceneDict = new Dictionary<int, Cutscene>();
	}
	
    void Start()
    {
        // populate list of actors in the scene
		GameObject[] actorArray = GameObject.FindGameObjectsWithTag("CutsceneActor");
		foreach(GameObject x in actorArray){
			// check to make sure the game object has an actor component, add it to actor list
			CutsceneActor actor = x.GetComponent<CutsceneActor>();
			if(actor != null){
				int id = actor.id;
				if(!actorsDict.ContainsKey(id)){
					actorsDict.Add(id, new List<CutsceneActor>());
				}
				actorsDict[id].Add(actor);
			}
		}
    }

    void Update()
    {
		// update timing of cutscene
        if(inCutscene){
			cutsceneTimer += Time.deltaTime;
			
			while(currentTaskIndex < currentCutscene.tasks.Length && cutsceneTimer >= currentTask.trigger_time){
				EvaluateTask(currentTask);
			}
			
			// check for end of cutscene
			if(cutsceneTimer >= cutsceneDuration){
				sceneManager.player.UnlockPlayer();
				inCutscene = false;
			}
		}
    }
	
	void EvaluateTask(CutsceneTask task){
		// check that the actors for the task exist
		if(actorsDict.ContainsKey(task.id)){
			// trigger event for all actors with the id for the task
			foreach(CutsceneActor actor in actorsDict[task.id]){
				actor.animate(task.anim_name);
			}
		}
		
		currentTaskIndex++;
		if(currentTaskIndex < currentCutscene.tasks.Length){
			currentTask = currentCutscene.tasks[currentTaskIndex];
		}
	}
	
	public void LoadCutscene(string cutsceneName){
		// load in json of cutscene into TextAsset
		var operation = Addressables.LoadAssetAsync<TextAsset>(cutsceneAddressHeader + cutsceneName + ".json");
		currentCutsceneTxt = operation.WaitForCompletion();
		
		// parse json into cutscene object
		Cutscene cs = JsonConvert.DeserializeObject<Cutscene>(currentCutsceneTxt.text);
		cutsceneDict.Add(cs.id, cs);
		
		// done parsing json, release asset out of memory
		Addressables.Release(operation);
	}
	
	public void PlayCutscene(int cutsceneId){
		currentCutscene = cutsceneDict[cutsceneId];
		cutsceneTimer = 0.0f;
		currentTaskIndex = 0;
		currentTask = currentCutscene.tasks[0];
		cutsceneDuration = currentCutscene.duration;
		inCutscene = true; 
		sceneManager.player.LockPlayer();
	}
}
