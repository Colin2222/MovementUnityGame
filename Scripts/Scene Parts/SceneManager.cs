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
	
	[System.NonSerialized]
    public PlayerHub player;
	
	public ItemRegistry itemRegistry;
	public string itemRegistryXml;
	
	public LevelRegistry levelRegistry;
	public string levelRegistryXml;
	
	public ProfileManager profileManager;
	public LevelSelectionManager levelSelectManager;
	public bool isHubWorld;
	
	public TimingManager timer;
	public float countdownTime;
	float countdownTimer;
	bool countingDownStart = false;
	public TextMeshProUGUI countdownText;
	
	public CinemachineVirtualCamera vcam;
	
    GameObject playerStateObjectTest;
    GameObject playerObjectTest;
	
	void Awake(){
		// load in items from data xml
		itemRegistry = ItemRegistry.Instance();
		itemRegistry.LoadItems(itemRegistryXml);
		
		// load in level list from data xml
		levelRegistry = LevelRegistry.Instance();
		levelRegistry.LoadLevels(levelRegistryXml);
	}
	
    // Start is called before the first frame update
    void Start()
    {
        // check if there is a DontDestroyOnLoad player, create a new one if there isnt
        playerObjectTest = GameObject.FindWithTag("Player");
        if(playerObjectTest == null){
            player = Instantiate(playerPrefab,new Vector3(0,0,0),Quaternion.identity).GetComponent<PlayerHub>();
        }
        else{
            player = playerObjectTest.GetComponent<PlayerHub>();
        }
		player.transform.position = playerSpawnTransform.position;
		vcam.m_Follow = player.transform;
		
		if(isHubWorld){
			profileManager.SetupProfileSelection(profileSelectionLocation);
			levelSelectManager.SetupLevelSelection(levelSelectionLocation);
			countdownText.gameObject.SetActive(false);
		} else{
			countdownTimer = countdownTime;
			countingDownStart = true;
			player.LockPlayer();
		}
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

    // loads the scene of the inputted build index
    public void SwitchScenes(int buildIndex)
    {
		Debug.Log("SWITCHING SCENES");
		
        //yield return new WaitForSeconds(transitionTime);

        UnityEngine.SceneManagement.SceneManager.LoadScene(buildIndex);
    }
}
