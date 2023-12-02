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
	public PlayerState state;
	public Animator animator;
	public PlayerReskinManager reskinner;
	public PlayerAttributeManager attributeManager;
	public PlayerSoundInterface soundInterface;
    // Start is called before the first frame update
    void Awake()
    {
		state = new PlayerState();
		SwitchPlayerSpritesheet("currentplayer");
    }
	
	void Start(){
		AudioClip bgAudio = GameObject.FindWithTag("SceneManager").GetComponent<SceneManager>().bgAudio;
		
		if(bgAudio != null){
			soundInterface.SetBackgroundAudio(bgAudio);
		}
	}

    // Update is called once per frame
    void Update()
    {
        
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
	
	public void InvertPlayerOutline(){
		
	}
}
