using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHub : MonoBehaviour
{
	public Rigidbody2D rigidbody;
	public CharacterPhysicsChecker physics;
	public PlayerInputManager inputManager;
	public PlayerStateManager stateManager;
	public CornerHandler cornerHandler;
	public PersistentState persistentState;
	public Animator animator;
	public PlayerReskinManager reskinner;
	public PlayerAttributeManager attributeManager;
	public PlayerSoundInterface soundInterface;
	
	[System.NonSerialized]
	public bool isSpawning = false;
	public float spawnTime;
	
    // Start is called before the first frame update
    void Awake()
    {
		SwitchPlayerSpritesheet("currentplayer");
    }
	
	void Start(){
		DontDestroyOnLoad(gameObject);
		
		AudioClip bgAudio = GameObject.FindWithTag("SceneManager").GetComponent<SceneManager>().bgAudio;
		
		if(bgAudio != null){
			soundInterface.SetBackgroundAudio(bgAudio);
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void FixedUpdate(){
		
	}
	
	public void LockPlayer(){
		inputManager.LockPlayer();
	}
	
	public void UnlockPlayer(){
		inputManager.UnlockPlayer();
	}
	
	public void SwitchPlayerSpritesheet(string spritesheetName){
		reskinner.SetNewSpritesheet(spritesheetName);
	}
	
	public void InvertPlayerOutline(bool inverted){
		reskinner.SetPlayerInvertSkin(inverted);
	}
	
	public IEnumerator RunSpawnBufferTimer(){
        isSpawning = true;
		yield return new WaitForSeconds(spawnTime);
		isSpawning = false;
    }
}
