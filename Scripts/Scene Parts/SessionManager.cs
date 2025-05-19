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
	[System.NonSerialized]
	public int currentWalkEntraceDirection = 0;
	int currentDirectionNumber;

	public GameSaveData saveData;
	public string currentPlayerName = "player";
	string addressHeader = "Assets/Data/SaveData/";

	public GameDialogueData dialogueData;
	string dialogueAddress = "Assets/Data/Dialogue/game_dialogue.txt";

	public RoomMappingData roomMappingData;
	string roomMappingAddress = "Assets/Data/room_mapping_data.txt";

	void Awake()
	{
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

	public void LoadRoomMappings()
	{
		var operation = Addressables.LoadAssetAsync<TextAsset>(roomMappingAddress);
		TextAsset txtAsset = operation.WaitForCompletion();

		JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
		roomMappingData = JsonConvert.DeserializeObject<RoomMappingData>(txtAsset.text, settings);

		Addressables.Release(operation);
	}

	public void LoadAnchorPointMappings()
	{

	}

	public void LoadDialogue()
	{
		var operation = Addressables.LoadAssetAsync<TextAsset>(dialogueAddress);
		TextAsset txtAsset = operation.WaitForCompletion();

		JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
		dialogueData = JsonConvert.DeserializeObject<GameDialogueData>(txtAsset.text, settings);
		//Debug.Log(((DialogueNodeText)(dialogueData.npc_dialogues["blacksmith"].branches[0].nodes[0])).text);

		Addressables.Release(operation);
	}

	public DialogueTree GetDialogueTree(string npcName)
	{
		return dialogueData.npc_dialogues[npcName];
	}

	public void LoadData()
	{
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

	public void SaveData()
	{
		// load in json of player's save into TextAsset
		var operation = Addressables.LoadAssetAsync<TextAsset>(addressHeader + currentPlayerName + "_save.txt");
		TextAsset txtAsset = operation.WaitForCompletion();

		// serialize saveData into json
		string jsonText = JsonConvert.SerializeObject(saveData, Formatting.Indented);
		File.WriteAllText(addressHeader + currentPlayerName + "_save.txt", jsonText);

		// done parsing json, release asset out of memory
		Addressables.Release(operation);
	}

	public bool GetData(string dataKey)
	{
		bool result = saveData.progress_markers[dataKey];
		if (result != null)
		{
			return result;
		}
		return false;
	}

	public void SetData(string dataKey, bool dataValue)
	{
		saveData.progress_markers[dataKey] = dataValue;
	}

	public void SetCurrentRoom()
	{
		saveData.rooms[sceneManager.sceneName] = sceneManager.siteManager.SaveSites();
		saveData.rooms[sceneManager.sceneName].npcs = sceneManager.npcManager.SaveNPCs();
		saveData.rooms[sceneManager.sceneName].fills = sceneManager.fillManager.SaveFills();
	}

	public void SetRoomItems()
	{
		saveData.rooms[sceneManager.sceneName].world_items = sceneManager.itemManager.GetWorldItems();
	}

	public void SetPlayerInventory(SavedInventory inventory)
	{
		saveData.player_inventory = inventory;
	}

	public void SetCableCar(int anchorPointIndex)
	{
		currentEntranceDirection = saveData.integer_markers["cable_car_location"] < anchorPointIndex ? -1 : 1;
		saveData.integer_markers["cable_car_location"] = anchorPointIndex;
	}

	public bool CheckCableCar(int anchorPointIndex)
	{
		return saveData.integer_markers["cable_car_location"] == anchorPointIndex;
	}

	public void SetSite(SavedSite site, int siteSlot)
	{
		int siteIndex = FindSiteSlotIndex(saveData.rooms[sceneManager.sceneName], siteSlot);
		saveData.rooms[sceneManager.sceneName].site_slots[siteIndex].site = site;
	}

	int FindSiteSlotIndex(SavedRoom room, int siteId)
	{
		for (int i = 0; i < room.site_slots.Count; i++)
		{
			if (room.site_slots[i].id == siteId)
			{
				return i;
			}
		}
		return -1;
	}

	public void CreateNewSave()
	{
		// load in json of fresh save into TextAsset
		var operation = Addressables.LoadAssetAsync<TextAsset>(addressHeader + "new_save.txt");
		TextAsset freshSaveTxt = operation.WaitForCompletion();

		// parse json into cutscene object
		saveData = JsonConvert.DeserializeObject<GameSaveData>(freshSaveTxt.text);
		//Debug.Log(saveData.rooms["mountainbase_rightedge1"].site_slots[1].site.inventories.Count);

		// done parsing json, release asset out of memory
		Addressables.Release(operation);
	}

	public void TransitionScene(int buildIndex, int entranceNumber, int directionNumber)
	{
		if (!player.isSpawning)
		{
			// set entrance number
			this.currentEntranceNumber = entranceNumber;

			// trigger overlay and player animations
			this.currentWalkEntraceDirection = directionNumber;
			if (directionNumber == 1)
			{
				sceneManager.transitionManager.TransitionRight();
			}
			else
			{
				sceneManager.transitionManager.TransitionLeft();
			}

			// trigger audio fade out
			GameObject.FindWithTag("BackgroundSoundManager").GetComponent<BackgroundSoundManager>().SceneTransitionFadeOut();

			// cue player buffer time on spawning so they dont ping pong load between rooms
			StartCoroutine(player.RunSpawnBufferTimer());

			// load into new scene
			StartCoroutine(sceneManager.SwitchScenes(buildIndex));
		}
	}

	public void UpdateSceneManager(SceneManager sceneManager)
	{
		this.sceneManager = sceneManager;
	}

	public void UpdatePlayer(PlayerHub player)
	{
		this.player = player;
	}

	public void UpdateSpawnPoint(Transform transform)
	{
		sceneManager.playerSpawnTransform = transform;
	}


	// SAVE DATA INTERFACING METHODS
	public bool GetProgressMarker(string markerName)
	{
		if (saveData.progress_markers.ContainsKey(markerName))
		{
			return saveData.progress_markers[markerName];
		}
		return false;
	}
	public void SetProgressMarker(string markerName, bool value)
	{
		if (saveData.progress_markers.ContainsKey(markerName))
		{
			saveData.progress_markers[markerName] = value;
		}
		else
		{
			saveData.progress_markers.Add(markerName, value);
		}
	}

	public int GetIntegerMarker(string markerName)
	{
		if (saveData.integer_markers.ContainsKey(markerName))
		{
			return saveData.integer_markers[markerName];
		}
		return -1;
	}

	public void SetIntegerMarker(string markerName, int value)
	{
		if (saveData.integer_markers.ContainsKey(markerName))
		{
			saveData.integer_markers[markerName] = value;
		}
		else
		{
			saveData.integer_markers.Add(markerName, value);
		}
	}

	public bool GetFillIsActive(string fillName)
	{
		if (saveData.rooms[sceneManager.sceneName].fills.ContainsKey(fillName))
		{
			return saveData.rooms[sceneManager.sceneName].fills[fillName].active;
		}
		return false;
	}

	public bool GetFillIsActive(string fillName, string roomName)
	{
		if (saveData.rooms[roomName].fills.ContainsKey(fillName))
		{
			return saveData.rooms[roomName].fills[fillName].active;
		}
		return false;
	}

	public void SetFillIsActive(string fillName, bool value)
	{
		if (saveData.rooms[sceneManager.sceneName].fills.ContainsKey(fillName))
		{
			saveData.rooms[sceneManager.sceneName].fills[fillName].active = value;
		}
	}

	public void SetFillIsActive(string fillName, bool value, string roomName)
	{
		if (saveData.rooms[roomName].fills.ContainsKey(fillName))
		{
			saveData.rooms[roomName].fills[fillName].active = value;
		}
	}

	public Vector2 GetNPCPosition(string npcName)
	{
		if (saveData.rooms[sceneManager.sceneName].npcs != null)
		{
			for (int i = 0; i < saveData.rooms[sceneManager.sceneName].npcs.Count; i++)
			{
				if (saveData.rooms[sceneManager.sceneName].npcs[i].name == npcName)
				{
					return new Vector2(saveData.rooms[sceneManager.sceneName].npcs[i].x_pos, saveData.rooms[sceneManager.sceneName].npcs[i].y_pos);
				}
			}
		}
		return Vector2.zero;
	}

	public Vector2 GetNPCPosition(string npcName, string roomName)
	{
		if (saveData.rooms[roomName].npcs != null)
		{
			for (int i = 0; i < saveData.rooms[roomName].npcs.Count; i++)
			{
				if (saveData.rooms[roomName].npcs[i].name == npcName)
				{
					return new Vector2(saveData.rooms[roomName].npcs[i].x_pos, saveData.rooms[roomName].npcs[i].y_pos);
				}
			}
		}
		return Vector2.zero;
	}

	public void SetNPCPosition(string npcName, Vector2 position)
	{
		if (saveData.rooms[sceneManager.sceneName].npcs != null)
		{
			for (int i = 0; i < saveData.rooms[sceneManager.sceneName].npcs.Count; i++)
			{
				if (saveData.rooms[sceneManager.sceneName].npcs[i].name == npcName)
				{
					saveData.rooms[sceneManager.sceneName].npcs[i].x_pos = position.x;
					saveData.rooms[sceneManager.sceneName].npcs[i].y_pos = position.y;
					return;
				}
			}
		}
	}

	public void SetNPCPosition(string npcName, Vector2 position, string roomName)
	{
		if (saveData.rooms[roomName].npcs != null)
		{
			for (int i = 0; i < saveData.rooms[roomName].npcs.Count; i++)
			{
				if (saveData.rooms[roomName].npcs[i].name == npcName)
				{
					saveData.rooms[roomName].npcs[i].x_pos = position.x;
					saveData.rooms[roomName].npcs[i].y_pos = position.y;
					return;
				}
			}
		}
	}

	public string GetNPCDefaultAnimation(string npcName)
	{
		if (saveData.rooms[sceneManager.sceneName].npcs != null)
		{
			for (int i = 0; i < saveData.rooms[sceneManager.sceneName].npcs.Count; i++)
			{
				if (saveData.rooms[sceneManager.sceneName].npcs[i].name == npcName)
				{
					return saveData.rooms[sceneManager.sceneName].npcs[i].default_animation;
				}
			}
		}
		return null;
	}

	public string GetNPCDefaultAnimation(string npcName, string roomName)
	{
		if (saveData.rooms[roomName].npcs != null)
		{
			for (int i = 0; i < saveData.rooms[roomName].npcs.Count; i++)
			{
				if (saveData.rooms[roomName].npcs[i].name == npcName)
				{
					return saveData.rooms[roomName].npcs[i].default_animation;
				}
			}
		}
		return null;
	}

	public void SetNPCDefaultAnimation(string npcName, string animationName)
	{
		if (saveData.rooms[sceneManager.sceneName].npcs != null)
		{
			for (int i = 0; i < saveData.rooms[sceneManager.sceneName].npcs.Count; i++)
			{
				if (saveData.rooms[sceneManager.sceneName].npcs[i].name == npcName)
				{
					saveData.rooms[sceneManager.sceneName].npcs[i].default_animation = animationName;
					return;
				}
			}
		}
	}

	public void SetNPCDefaultAnimation(string npcName, string animationName, string roomName)
	{
		if (saveData.rooms[roomName].npcs != null)
		{
			for (int i = 0; i < saveData.rooms[roomName].npcs.Count; i++)
			{
				if (saveData.rooms[roomName].npcs[i].name == npcName)
				{
					saveData.rooms[roomName].npcs[i].default_animation = animationName;
					return;
				}
			}
		}
	}

	public void SetNPCDirection(string npcName, int direction)
	{
		if (saveData.rooms[sceneManager.sceneName].npcs != null)
		{
			for (int i = 0; i < saveData.rooms[sceneManager.sceneName].npcs.Count; i++)
			{
				if (saveData.rooms[sceneManager.sceneName].npcs[i].name == npcName)
				{
					saveData.rooms[sceneManager.sceneName].npcs[i].direction = direction;
					return;
				}
			}
		}
	}

	public void SetNPCDirection(string npcName, int direction, string roomName)
	{
		if (saveData.rooms[roomName].npcs != null)
		{
			for (int i = 0; i < saveData.rooms[roomName].npcs.Count; i++)
			{
				if (saveData.rooms[roomName].npcs[i].name == npcName)
				{
					saveData.rooms[roomName].npcs[i].direction = direction;
					return;
				}
			}
		}
	}

	public void AddWorldItem(string itemId, Vector2 position, string roomName)
	{
		SavedWorldItem worldItem = new SavedWorldItem();
		worldItem.item_id = itemId;
		worldItem.x_pos = position.x;
		worldItem.y_pos = position.y;

		saveData.rooms[roomName].world_items.Add(worldItem);
	}

	public void SetSiteAdditionalData(int siteId, string dataKey, string dataValue, string roomName)
	{
		if (saveData.rooms[roomName].site_slots != null)
		{
			for (int i = 0; i < saveData.rooms[roomName].site_slots.Count; i++)
			{
				if (saveData.rooms[roomName].site_slots[i].id == siteId)
				{
					saveData.rooms[roomName].site_slots[i].site.additional_data[dataKey] = dataValue;
					return;
				}
			}
		}
	}

	public string GetSiteName(int siteId, string roomName)
	{
		if (saveData.rooms[roomName].site_slots != null)
		{
			for (int i = 0; i < saveData.rooms[roomName].site_slots.Count; i++)
			{
				if (saveData.rooms[roomName].site_slots[i].id == siteId)
				{
					if (saveData.rooms[roomName].site_slots[i].site != null)
					{
						return saveData.rooms[roomName].site_slots[i].site.name;
					}
					else
					{
						Debug.LogError("Site is null");
						return null;
					}
				}
			}
		}
		return null;
	}

	public string GetAnchorRoomName(int anchorId)
	{
		if(roomMappingData.anchor_points.ContainsKey(anchorId.ToString()))
		{
			int buildIndex = roomMappingData.anchor_points[anchorId.ToString()].build_index;
			return roomMappingData.build_to_name[buildIndex.ToString()];
		}
		return "";
	}
}
