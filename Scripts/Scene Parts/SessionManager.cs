using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.AddressableAssets;
using Newtonsoft.Json;

// THIS CLASS WILL HOLD ALL INFORMATION RELEVANT TO THE CURRENT PLAY SESSION
// SAVE STATE, TRANSITIONS BETWEEN ROOM
public class SessionManager : MonoBehaviour
{
	public SceneManager sceneManager;
	public PlayerHub player;
	
	[System.NonSerialized]
	public int currentEntranceNumber = 0;
	int currentDirectionNumber;
	
	public GameSaveData saveData;
	public string currentPlayerName = "player";
	string addressHeader = "Assets/Data/SaveData/";
	
	void Awake(){
		DontDestroyOnLoad(gameObject);
	}
	
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void LoadData(){
		// load in json of player's save into TextAsset
		var operation = Addressables.LoadAssetAsync<TextAsset>(addressHeader + currentPlayerName + "_save.txt");
		TextAsset txtAsset = operation.WaitForCompletion();
		
		// parse json into cutscene object
		saveData = JsonConvert.DeserializeObject<GameSaveData>(txtAsset.text);
		//Debug.Log(saveData.rooms["mountainbase_rightedge1"].site_slots.Count);

		// done parsing json, release asset out of memory
		Addressables.Release(operation);
	}
	
	public void SaveData(){
		// load in json of player's save into TextAsset
		var operation = Addressables.LoadAssetAsync<TextAsset>(addressHeader + currentPlayerName + "_save.txt");
		TextAsset txtAsset = operation.WaitForCompletion();
		
		// serialize saveData into json
		string jsonText = JsonConvert.SerializeObject(saveData, Formatting.Indented);
		File.WriteAllText(addressHeader + currentPlayerName + "_save.txt", jsonText);
		
		// done parsing json, release asset out of memory
		Addressables.Release(operation);
	}
	
	public bool GetData(string dataKey){
		bool result = saveData.progress_markers[dataKey];
		if(result != null){
			return result;
		}
		return false;
	}
	
	public void SetData(string dataKey, bool dataValue){
		saveData.progress_markers[dataKey] = dataValue;
	}

	public void SetCurrentRoom(){
		saveData.rooms[sceneManager.sceneName] = sceneManager.siteManager.SaveSites();
	}

	public void SetRoomItems(){
		saveData.rooms[sceneManager.sceneName].world_items = sceneManager.itemManager.GetWorldItems();
	}

	public void LoadRoomItems(){
		List<SavedWorldItem> items = saveData.rooms[sceneManager.sceneName].world_items;
		sceneManager.itemManager.LoadWorldItems(items);
	}
	
	public void CreateNewSave(){
		// load in json of fresh save into TextAsset
		var operation = Addressables.LoadAssetAsync<TextAsset>(addressHeader + "new_save.txt");
		TextAsset freshSaveTxt = operation.WaitForCompletion();
		
		// parse json into cutscene object
		saveData = JsonConvert.DeserializeObject<GameSaveData>(freshSaveTxt.text);
		//Debug.Log(saveData.rooms["mountainbase_rightedge1"].site_slots[1].site.inventories.Count);
		
		// done parsing json, release asset out of memory
		Addressables.Release(operation);
	}
	
	public void TransitionScene(int buildIndex, int entranceNumber, int directionNumber){
		if(!player.isSpawning){
			// set entrance number
			this.currentEntranceNumber = entranceNumber;
			
			// trigger overlay and player animations
			sceneManager.transitionManager.ExitTransition(buildIndex);

			// cue player buffer time on spawning so they dont ping pong load between rooms
			StartCoroutine(player.RunSpawnBufferTimer());

			// load into new scene
			StartCoroutine(sceneManager.SwitchScenes(buildIndex));
		}
	}
	
	public void UpdateSceneManager(SceneManager sceneManager){
		this.sceneManager = sceneManager;
	}
	
	public void UpdatePlayer(PlayerHub player){
		this.player = player;
	}
	
	public void UpdateSpawnPoint(Transform transform){
		sceneManager.playerSpawnTransform = transform;
	}
}
