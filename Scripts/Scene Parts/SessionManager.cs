using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// THIS CLASS WILL HOLD ALL INFORMATION RELEVANT TO THE CURRENT PLAY SESSION
// SAVE STATE, TRANSITIONS BETWEEN ROOM
public class SessionManager : MonoBehaviour
{
	public SceneManager sceneManager;
	public PlayerHub player;
	
	[System.NonSerialized]
	public int currentEntranceNumber = 0;
	int currentDirectionNumber;
	
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
