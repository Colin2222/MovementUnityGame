using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.AddressableAssets;
using Newtonsoft.Json;
using DialogueDataClasses;

// THIS CLASS WILL HOLD ALL INFORMATION RELEVANT TO THE CURRENT PLAY SESSION
// SAVE STATE, TRANSITIONS BETWEEN ROOM
public class SessionManager : MonoBehaviour
{
	public static SessionManager Instance { get; private set; }
	public SceneManager sceneManager;
	public PlayerHub player;
	
	[System.NonSerialized]
	public int currentEntranceNumber = 0;
	[System.NonSerialized]
	public int currentEntranceDirection = 0;
	int currentDirectionNumber;
	
	public GameSaveData saveData;
	public string currentPlayerName = "player";
	string addressHeader = "Assets/Data/SaveData/";

	public GameDialogueData dialogueData;
	string dialogueAddress = "Assets/Data/Dialogue/game_dialogue.txt";

	public RoomMappingData roomMappingData;
	string roomMappingAddress =  "Assets/Data/room_mapping_data.txt";
	
	void Awake(){
		// SINGLETON PATTERN
		if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
		// assign this instance as singleton
        Instance = this;

		DontDestroyOnLoad(gameObject);
		LoadData();
		LoadDialogue();
		LoadRoomMappings();
	}
	
    // Start is called before the first frame update
    void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void LoadRoomMappings(){
		var operation = Addressables.LoadAssetAsync<TextAsset>(roomMappingAddress);
		TextAsset txtAsset = operation.WaitForCompletion();

		JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
		roomMappingData = JsonConvert.DeserializeObject<RoomMappingData>(txtAsset.text, settings);

		Addressables.Release(operation);
	}

	public void LoadDialogue(){
		var operation = Addressables.LoadAssetAsync<TextAsset>(dialogueAddress);
		TextAsset txtAsset = operation.WaitForCompletion();

		JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
		dialogueData = JsonConvert.DeserializeObject<GameDialogueData>(txtAsset.text, settings);
		//Debug.Log(((DialogueNodeText)(dialogueData.npc_dialogues["blacksmith"].branches[0].nodes[0])).text);

		Addressables.Release(operation);
	}

	public DialogueTree GetDialogueTree(string npcName){
		return dialogueData.npc_dialogues[npcName];
	}
	
	public void LoadData(){
		// load in json of player's save into TextAsset
		Addressables.ClearDependencyCacheAsync(addressHeader + currentPlayerName + "_save.txt");
		Caching.ClearCache();
		AssetDatabase.Refresh(); /* this is the effective one (among the above 2). It also isnt allowed to be in build, since AssetDatabase is under the UnityEditor namespace, which is not used in built versions */
		var operation = Addressables.LoadAssetAsync<TextAsset>(addressHeader + currentPlayerName + "_save.txt");
		TextAsset txtAsset = operation.WaitForCompletion();
		
		// parse json into cutscene object
		saveData = JsonConvert.DeserializeObject<GameSaveData>(txtAsset.text);
		//Debug.Log(saveData.rooms["guidestone_cave"].site_slots);

		// done parsing json, release asset out of memory
		Addressables.Release(operation);

		// sync cable car location to get entrance direction
		SetCableCar(saveData.integer_markers["cable_car_location"]);
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

	public void SetPlayerInventory(SavedInventory inventory){
		saveData.player_inventory = inventory;
	}

	public void SetCableCar(int anchorPointIndex){
		currentEntranceDirection = saveData.integer_markers["cable_car_location"] < anchorPointIndex ? -1 : 1;
		saveData.integer_markers["cable_car_location"] = anchorPointIndex;
	}

	public bool CheckCableCar(int anchorPointIndex){
		return saveData.integer_markers["cable_car_location"] == anchorPointIndex;
	}

	public void SetSite(SavedSite site, int siteSlot){
		int siteIndex = FindSiteSlotIndex(saveData.rooms[sceneManager.sceneName], siteSlot);
		saveData.rooms[sceneManager.sceneName].site_slots[siteIndex].site = site;
	}

	public int GetIntegerMarker(string markerName){
		return saveData.integer_markers[markerName];
	}

	public void SetIntegerMarker(string markerName, int value){
		saveData.integer_markers[markerName] = value;
	}

	int FindSiteSlotIndex(SavedRoom room, int siteId){
		for(int i = 0; i < room.site_slots.Count; i++){
			if(room.site_slots[i].id == siteId){
				return i;
			}
		}
		return -1;
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
