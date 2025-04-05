using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHub : MonoBehaviour
{
	public static PlayerHub Instance { get; private set; }
	public Rigidbody2D rigidbody;
	public CharacterPhysicsChecker physics;
	public PlayerInputManager inputManager;
	public PlayerStateManager stateManager;
	public CornerHandler cornerHandler;
	public PersistentState persistentState;
	public Animator animator;
	public PlayerReskinManager reskinner;
	public PlayerAttributeManager attributeManager;
	public PlayerInventoryHandler inventoryHandler;
	public PlayerInteractor interactor;
	public PlayerSoundInterface soundInterface;
	public PlayerOverrideManager overrideManager;
	
	[System.NonSerialized]
	public bool isSpawning = false;
	public float spawnTime;
	
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

		SwitchPlayerSpritesheet("currentplayer");
    }
	
	void Start(){
		SceneManager sceneManager = GameObject.FindWithTag("SceneManager").GetComponent<SceneManager>();
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
