using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class SceneManager : MonoBehaviour
{	
	public static SceneManager Instance { get; private set; }

	public string sceneName;
	public Transform playerSpawnTransform;
	public Transform profileSelectionLocation;
	public Transform levelSelectionLocation;
	
	public float transitionTime;
	
	public GameObject playerPrefab;
	public GameObject profileManagerPrefab;
	
	[System.NonSerialized]
    public PlayerHub player;
	
	public ItemRegistry itemRegistry;
	public string itemRegistryXml;
	
	public LevelRegistry levelRegistry;
	public string levelRegistryXml;
	
	[System.NonSerialized]
	public JournalManager journalManager;
	public GameObject journalManagerPrefab;
	
	public ProfileManager profileManager;
	public LevelSelectionManager levelSelectManager;
	public bool isHubWorld;
	
	public TimingManager timer;
	public float countdownTime;
	float countdownTimer;
	bool countingDownStart = false;
	public TextMeshProUGUI countdownText;
	public TextMeshProUGUI profileText;
	
	public CinemachineVirtualCamera vcam;
	float originalCameraDistance;
	GameObject cameraAimObj;
	
	public int hubWorldIndex;
	
	public SceneTransitionManager transitionManager;
	
	public PersistentState persistentState;
	public GameObject persistentStatePrefab;
	
	public CutsceneManager cutsceneManager;
	
	[System.NonSerialized]
	public SessionManager sessionManager;
	public GameObject sessionManagerPrefab;
	
	public Color obstacleColor;
	public Color backgroundColor;
	public bool invertPlayerColor;

	public GameObject sitePrefabRegistryPrefab;
	public SitePrefabRegistry sitePrefabRegistry;
	
	public SoundManager soundManager;

	public SiteManager siteManager;
	public ItemManager itemManager;
	public DialogueManager dialogueManager;

	[System.NonSerialized]
	public GameObject mainCameraObj;
	
    GameObject playerStateObjectTest;
    GameObject playerObjectTest;
    GameObject persistentStateTest;
	GameObject sessionManagerTest;
	GameObject mainCameraTest;
	
	void Awake(){
		// SINGLETON PATTERN
		if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
		// assign this instance as singleton
        Instance = this;

		// load in items from data xml
		itemRegistry = ItemRegistry.Instance();
		itemRegistry.LoadItems(itemRegistryXml);
		
		// load in level list from data xml
		levelRegistry = LevelRegistry.Instance();
		levelRegistry.LoadLevels(levelRegistryXml);
		
		// check if there is a DontDestroyOnLoad session manager, create a new one if there isnt
		sessionManagerTest = GameObject.FindWithTag("SessionManager");
        if(sessionManagerTest == null){
            sessionManager = Instantiate(sessionManagerPrefab,new Vector3(0,0,0),Quaternion.identity).GetComponent<SessionManager>();
        }
        else{
            sessionManager = sessionManagerTest.GetComponent<SessionManager>();
        }
		sessionManager.UpdateSceneManager(this);
		
		// check if there is a DontDestroyOnLoad player, create a new one if there isnt
        playerObjectTest = GameObject.FindWithTag("Player");
        if(playerObjectTest == null){
            player = Instantiate(playerPrefab,new Vector3(0,0,0),Quaternion.identity).GetComponent<PlayerHub>();
        }
        else{
            player = playerObjectTest.GetComponent<PlayerHub>();
        }
		if(vcam != null){
			vcam.m_Follow = player.transform;
		}
		sessionManager.UpdatePlayer(player);
		
		// check if there is a DontDestroyOnLoad profile manager, create a new one if there isnt
        GameObject profileManagerObjectTest = GameObject.FindWithTag("ProfileManager");
		if(profileManagerObjectTest == null){
            profileManager = Instantiate(profileManagerPrefab,new Vector3(0,0,0),Quaternion.identity).GetComponent<ProfileManager>();
        }
		else{
            profileManager = profileManagerObjectTest.GetComponent<ProfileManager>();
        }
		profileManager.SetNameTextRef(profileText);
		profileManager.SetPlayerRef(player);
		profileManager.ResetPlayerSpritesheet();
		
		// check if there is a DontDestroyOnLoad journal manager, create a new one if there isnt
        GameObject journalManagerObjectTest = GameObject.FindWithTag("JournalManager");
		if(journalManagerObjectTest == null){
            journalManager = Instantiate(journalManagerPrefab,new Vector3(0,0,0),Quaternion.identity).GetComponent<JournalManager>();
        }
		else{
            journalManager = journalManagerObjectTest.GetComponent<JournalManager>();
        }

		// check if there is a DontDestroyOnLoad SitePrefabRegistry, create a new one if there isnt
		GameObject sitePrefabRegistryTest = GameObject.FindWithTag("SitePrefabRegistry");
		if(sitePrefabRegistryTest == null){
			sitePrefabRegistry = Instantiate(sitePrefabRegistryPrefab,new Vector3(0,0,0),Quaternion.identity).GetComponent<SitePrefabRegistry>();
		}
		else{
			sitePrefabRegistry = sitePrefabRegistryTest.GetComponent<SitePrefabRegistry>();
		}
		
		player.InvertPlayerOutline(invertPlayerColor);
	}
	
    // Start is called before the first frame update
    void Start()
    {	
		profileManager.SetupProfileSelection(profileSelectionLocation);
		if(isHubWorld){
			levelSelectManager.SetupLevelSelection(levelSelectionLocation);
			countdownText.gameObject.SetActive(false);
		} else{
			// send profilemanager to timer before all the action starts
			timer.profileManager = profileManager;
			
			/*
			countdownTimer = countdownTime;
			countingDownStart = true;
			player.LockPlayer();
			*/
		}
		
		if(sessionManager.currentWalkEntraceDirection == 1){
			transitionManager.EnterTransition(1);
		} else{
			transitionManager.EnterTransition(-1);
		}
		
		GameObject[] colorChanges = GameObject.FindGameObjectsWithTag("LevelBlock");
		foreach(GameObject block in colorChanges){
			block.GetComponent<SpriteRenderer>().color = obstacleColor;
		}
		GameObject.FindWithTag("MainCamera").GetComponent<Camera>().backgroundColor = backgroundColor;
		mainCameraObj = GameObject.FindWithTag("MainCamera");
		
		journalManager.SeekUI();
		
		//cutsceneManager.LoadCutscene("room_0_c0");
		//cutsceneManager.PlayCutscene(1);

		player.inventoryHandler.SceneSwitchReset();
    }
	
	void Update(){
		// handle prelevel countdown
		if(countingDownStart){
			countdownTimer -= Time.deltaTime;
			countdownText.text = ((int)(Mathf.Ceil(countdownTimer))).ToString();
			if(countdownTimer <= 0){
				countdownText.gameObject.SetActive(false);
				countingDownStart = false;
				player.UnlockPlayer();
				timer.StartTimer();
			}
		}
	}

    // loads the scene of the inputted build index with delay 
    public IEnumerator SwitchScenes(int buildIndex)
    {
		sessionManager.SetCurrentRoom();
		sessionManager.SetRoomItems();
		sessionManager.SetPlayerInventory(player.inventoryHandler.inventory.SaveInventory());
		//sessionManager.SaveData();

		Debug.Log("SWITCHING SCENES");
		
        yield return new WaitForSeconds(transitionTime);

		player.stateManager.ExitTransformFollow();
        UnityEngine.SceneManagement.SceneManager.LoadScene(buildIndex);
    }
	
	public void ReturnToHub(){
		transitionManager.ExitTransition(hubWorldIndex);
	}
	
	public void RestartLevel(){
		transitionManager.ExitTransition(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
	}

	public SavedRoom GetCurrentRoomSave(){
		return sessionManager.saveData.rooms[sceneName];
	}

	public void SetCamera(GameObject cameraTargetObj, float distance){
		cameraAimObj = cameraTargetObj;
		vcam.m_Follow = cameraTargetObj.transform;
		originalCameraDistance = vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance;
		vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = distance;
	}

	public void ResetCamera(){
		vcam.m_Follow = player.transform;
		vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = originalCameraDistance;
		UnityEngine.Object.Destroy(cameraAimObj);
	}
}
