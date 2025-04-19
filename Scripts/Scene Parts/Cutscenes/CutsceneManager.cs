using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Newtonsoft.Json;
using Cinemachine;

public class CutsceneManager : MonoBehaviour
{
	float cutsceneTimer;
	float cutsceneDuration;
	string cutsceneAddressHeader = "Assets/Data/Cutscenes/";
	TextAsset currentCutsceneTxt;
	
	public CinemachineVirtualCamera vcam;
	
	[System.NonSerialized]
	public bool inCutscene = false;
	[System.NonSerialized]
	public bool inDialogue = false;
	[System.NonSerialized]
	public bool playerLocked = false;
	bool inFullscreenCutscene = false;
	bool fullscreenCutsceneEntering = false;
	bool fullscreenCutsceneExiting = false;
	string animationName;
	
	Cutscene currentCutscene;
	int currentTaskIndex;
	CutsceneTask currentTask;
	Dictionary<int, List<CutsceneActor>> actorsDict;
	Dictionary<int, Cutscene> cutsceneDict;
	Dictionary<int, Transform> cameraAnchorPoints;
	int lastCutsceneLoaded = 0;
	
	public SceneManager sceneManager;
	public CutsceneManagerInteractable interactable;
	public Animator fullscreenAnimator;
	float fullscreenCutsceneBlackoutTime;
	public SpriteRenderer fullscreenCutsceneBlackoutSprite;
	public SpriteRenderer fullscreenCutsceneSprite;
	public SpriteRenderer fullscreenCutsceneBackgroundSprite;
	GameObject mainCameraObj;
	
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
				
				// deactivate actor's gameobject if designated as deactivated
				if(actor.deactivatedOnStart){
					actor.gameObject.SetActive(false);
				}
				
				actor.cutsceneManager = this;
			}
		}

		fullscreenAnimator.gameObject.SetActive(false);

		// get the camera gameobject
		GameObject cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
		if(cameraObj != null){
			mainCameraObj = cameraObj;
		}

    }

    void Update()
    {
		// update timing of cutscene
        if(inCutscene){
			if(!inDialogue){
				cutsceneTimer += Time.deltaTime;
					while(currentTaskIndex < currentCutscene.tasks.Length && cutsceneTimer >= currentTask.trigger_time){
					EvaluateTask(currentTask);
				}
				
				// check for end of cutscene
				if(cutsceneTimer >= cutsceneDuration){
					sceneManager.player.UnlockPlayer();
					sceneManager.player.inputManager.ExitCutscene();
					inCutscene = false;
					currentCutscene.active = false;
					SwitchCameraAnchor(sceneManager.player.gameObject.transform);
				}
			}
		} else if(inFullscreenCutscene){
			HandleFullscreenCutscene();
		}
    }
	
	void EvaluateTask(CutsceneTask task){
		// check that the actors for the task exist
		if(actorsDict.ContainsKey(task.id)){
			// trigger event for all actors with the id for the task
			foreach(CutsceneActor actor in actorsDict[task.id]){
				// do any custom actions
				foreach(CustomAction action in task.custom_actions){
					MethodInfo method = actor.GetType().GetMethod(action.name);
					// pass string parameters if exists, otherwise pass float parameters
					if(action.string_parameters != null && action.string_parameters.Length > 0){
						object[] sParameters = new object[action.string_parameters.Length];
						Array.Copy(action.string_parameters, sParameters, action.string_parameters.Length);
						method.Invoke(actor, sParameters);
					} else{
						object[] oParameters = new object[action.parameters.Length];
						Array.Copy(action.parameters, oParameters, action.parameters.Length);
						method.Invoke(actor, oParameters);
					}
				}
				
				// do the animation
				if(task.anim_name != ""){
					actor.animate(task.anim_name);
				}
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
		cs.active = false;
		if(!cutsceneDict.ContainsKey(cs.id)){
			cutsceneDict.Add(cs.id, cs);
		}
		lastCutsceneLoaded = cs.id;
		
		// done parsing json, release asset out of memory
		Addressables.Release(operation);
	}
	
	public void PlayCutscene(int cutsceneId){
		if(cutsceneId == -1){
			cutsceneId = lastCutsceneLoaded;
		}
		currentCutscene = cutsceneDict[cutsceneId];
		currentCutscene.active = true;
		cutsceneTimer = 0.0f;
		currentTaskIndex = 0;
		currentTask = currentCutscene.tasks[0];
		cutsceneDuration = currentCutscene.duration;
		inCutscene = true; 

		playerLocked = currentCutscene.lock_player;
		if(playerLocked){
			sceneManager.player.stateManager.ResetPlayer();
			sceneManager.player.inputManager.EnterCutscene();
			sceneManager.player.LockPlayer();
		}
	}
	
	public void SwitchCameraAnchor(Transform anchor){
		if(vcam != null){
			vcam.m_Follow = anchor;
		}
	}

	public void EnterDialogue(){
		sceneManager.player.stateManager.EnterCutsceneDialogue(interactable);
		sceneManager.player.inputManager.inUI = true;
		inDialogue = true;
	}

	public void ExitDialogue(){
		sceneManager.player.stateManager.ResetPlayerNoAnim();
		sceneManager.player.inputManager.EnterCutscene();
		sceneManager.player.LockPlayer();
		sceneManager.player.inputManager.inUI = false;
		inDialogue = false;
	}

	public void StartFullscreenCutscene(float duration, float blackoutDuration, string animationName, string backgroundName){
		inFullscreenCutscene = true;
		cutsceneTimer = 0.0f;
		cutsceneDuration = duration;
		fullscreenCutsceneBlackoutTime = blackoutDuration;
		fullscreenCutsceneEntering = true;
		fullscreenAnimator.gameObject.SetActive(true);
		fullscreenAnimator.transform.position = new Vector3(sceneManager.mainCameraObj.transform.position.x, sceneManager.mainCameraObj.transform.position.y, 0.0f);
		this.animationName = animationName;
		fullscreenAnimator.Play(animationName, 0, 0.0f);
		fullscreenAnimator.speed = 0.0f;
		fullscreenAnimator.transform.position = new Vector3(sceneManager.mainCameraObj.transform.position.x, sceneManager.mainCameraObj.transform.position.y, 0.0f);

		// load background sprite
		var operation = Addressables.LoadAssetAsync<Sprite>("Assets/Data/Cutscenes/backgrounds/" + backgroundName + ".png");
		Sprite backgroundSprite = operation.WaitForCompletion();
		fullscreenCutsceneBackgroundSprite.sprite = backgroundSprite;
	}

	void HandleFullscreenCutscene(){
		fullscreenAnimator.transform.position = new Vector3(sceneManager.mainCameraObj.transform.position.x, sceneManager.mainCameraObj.transform.position.y, sceneManager.mainCameraObj.transform.position.z + 16.0f);

		cutsceneTimer += Time.deltaTime;

		if(fullscreenCutsceneEntering){
			float a = cutsceneTimer / fullscreenCutsceneBlackoutTime;
			fullscreenCutsceneBlackoutSprite.color = new Color(0.0f, 0.0f, 0.0f, Mathf.Clamp(a, 0.0f, 1.0f));
			fullscreenCutsceneSprite.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Clamp(a, 0.0f, 1.0f));
			fullscreenCutsceneBackgroundSprite.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Clamp(a, 0.0f, 1.0f));
			if(cutsceneTimer >= fullscreenCutsceneBlackoutTime){
				cutsceneTimer = 0.0f;
				fullscreenCutsceneEntering = false;
				fullscreenCutsceneBlackoutSprite.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
				fullscreenCutsceneSprite.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
				fullscreenCutsceneBackgroundSprite.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
				inFullscreenCutscene = true;
				fullscreenAnimator.speed = 1.0f;
			}
		} else if(fullscreenCutsceneExiting){
			float a = cutsceneTimer / fullscreenCutsceneBlackoutTime;
			fullscreenCutsceneBlackoutSprite.color = new Color(0.0f, 0.0f, 0.0f, Mathf.Clamp(1.0f - a, 0.0f, 1.0f));
			fullscreenCutsceneSprite.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Clamp(1.0f - a, 0.0f, 1.0f));
			fullscreenCutsceneBackgroundSprite.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Clamp(1.0f - a, 0.0f, 1.0f));
			if(cutsceneTimer >= fullscreenCutsceneBlackoutTime){
				fullscreenCutsceneExiting = false;
				fullscreenCutsceneBlackoutSprite.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
				fullscreenCutsceneSprite.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
				fullscreenCutsceneBackgroundSprite.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
				fullscreenAnimator.gameObject.SetActive(false);
				inFullscreenCutscene = false;
			}
		} else{
			if(cutsceneTimer >= cutsceneDuration){
				fullscreenCutsceneExiting = true;
				cutsceneTimer = 0.0f;
				fullscreenAnimator.speed = 0.0f;
			}
		}
	}
}
