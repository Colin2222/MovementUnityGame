using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class SceneManager : MonoBehaviour
{	
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
	
	public AudioClip bgAudio;
	
    GameObject playerStateObjectTest;
    GameObject playerObjectTest;
    GameObject persistentStateTest;
	GameObject sessionManagerTest;
	
	void Awake(){
		// load in items from data xml
		itemRegistry = ItemRegistry.Instance();
		itemRegistry.LoadItems(itemRegistryXml);
		
		// load in level list from data xml
		levelRegistry = LevelRegistry.Instance();
		levelRegistry.LoadLevels(levelRegistryXml);
		
		// check if there is a DontDestroyOnLoad persistent state
		// used for keeping track of serializable game save object (see SaveDataClasses and PersistentState scripts)
		// used for determining which door the player will enter from
		persistentStateTest = GameObject.FindWithTag("PersistentState");
        if(persistentStateTest == null){
            persistentState = Instantiate(persistentStatePrefab,new Vector3(0,0,0),Quaternion.identity).GetComponent<PersistentState>();
			
			//CREATE NEW SAVE, THIS IS TEMPORARY
			persistentState.CreateNewSave();
			persistentState.WriteSave();
        }
        else{
            persistentState = persistentStateTest.GetComponent<PersistentState>();
        }
		
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
		vcam.m_Follow = player.transform;
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
		
		player.InvertPlayerOutline(invertPlayerColor);
	}
	
    // Start is called before the first frame update
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
		
		transitionManager.EnterTransition();
		
		GameObject[] colorChanges = GameObject.FindGameObjectsWithTag("LevelBlock");
		foreach(GameObject block in colorChanges){
			block.GetComponent<SpriteRenderer>().color = obstacleColor;
		}
		GameObject.FindWithTag("MainCamera").GetComponent<Camera>().backgroundColor = backgroundColor;
		
		journalManager.SeekUI();
		
		//cutsceneManager.LoadCutscene("room_0_c0");
		//cutsceneManager.PlayCutscene(1);
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
		Debug.Log("SWITCHING SCENES");
		
        yield return new WaitForSeconds(transitionTime);

        UnityEngine.SceneManagement.SceneManager.LoadScene(buildIndex);
    }
	
	public void ReturnToHub(){
		transitionManager.ExitTransition(hubWorldIndex);
	}
	
	public void RestartLevel(){
		transitionManager.ExitTransition(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
	}
}
