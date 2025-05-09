using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneActor : MonoBehaviour
{
    public Animator animator;
	public int id;
	public Rigidbody2D rb;
	public SpriteRenderer spriteRenderer;
	public bool deactivatedOnStart;
	
	float tempGrav;
	bool gravityPaused = false;
	Vector2 cutsceneVelocity;
	float targetX;
	bool movingToTarget;
	int targetDirection;
	public string defaultAnim;
	public CutsceneManager cutsceneManager;
	public bool isPlayer;
	
	// Start is called before the first frame update
    void Start()
    {
        cutsceneVelocity = Vector2.zero;

		if(cutsceneManager == null){
			cutsceneManager = GameObject.FindWithTag("SceneManager").GetComponent<SceneManager>().cutsceneManager;
		}
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		if(cutsceneManager.inCutscene && (cutsceneManager.playerLocked || !isPlayer) && rb != null){
			if(movingToTarget){
				if((targetDirection == 1 && rb.transform.position.x > targetX) || (targetDirection == -1 && rb.transform.position.x < targetX)){
					cutsceneVelocity = new Vector2(0.0f, cutsceneVelocity.y);
					animate(defaultAnim);
					movingToTarget = false;
				}
			} 
			rb.velocity = cutsceneVelocity;
		}
    }
	
	public void animate(string animation){
		// TRY AND IMPLEMENT A TRY CATCH HERE, need to figure out type of error when invalid animation string given
		animator.Play(animation);
	}
	
	public void SetHorizontalVelocity(float velocity){
		cutsceneVelocity = new Vector2(velocity, cutsceneVelocity.y);
	}
	
	public void SetHorizontalVelocityToTarget(float velocity, float targetX){
		if(targetX > rb.transform.position.x){
			targetDirection = 1;
		} else{
			targetDirection = -1;
		}
		
		this.targetX = targetX;
		movingToTarget = true;
		cutsceneVelocity = new Vector2(velocity, cutsceneVelocity.y);
	}
	
	public void VerticalShift(float shift){
		Transform trans = rb.gameObject.transform;
		trans.position = new Vector3(trans.position.x, trans.position.y + shift, trans.position.z);
	}
	
	public void SetCoordinates(float x, float y){
		Transform trans = rb.gameObject.transform;
		trans.position = new Vector3(x, y, trans.position.z);
	}
	
	public void FaceLeft(){
		Transform trans = rb.gameObject.transform;
		trans.eulerAngles = new Vector2(0,180);
	}
	
	public void FaceRight(){
		Transform trans = rb.gameObject.transform;
		trans.eulerAngles = new Vector2(0,0);
	}
	
	public void ToggleActivation(){
		gameObject.SetActive(!(gameObject.activeInHierarchy));
	}
	
	public void ToggleGravity(){
		if(gravityPaused){
			rb.gravityScale = tempGrav;
			gravityPaused = false;
		} else{
			tempGrav = rb.gravityScale;
			rb.gravityScale = 0.0f;
			gravityPaused = true;
		}
	}
	
	public void SetSpriteOrder(float orderF){
		int order = (int)orderF;
		spriteRenderer.sortingOrder = order;
	}
	
	public void SetCameraAnchorPoint(){
		cutsceneManager.SwitchCameraAnchor(gameObject.transform);
	}

	// only available for one-per-scene DialogueMananger, cutscene actor -1
	public void StartDialogue(string DialogueCode){
		this.transform.parent.GetComponent<DialogueManager>().StartDialogue(DialogueCode);
	}

	public void PlaySong(string songName){
		GameObject bgsmObj = GameObject.FindWithTag("BackgroundSoundManager");
		if(bgsmObj == null) return;
		BackgroundSoundManager bgsm = bgsmObj.GetComponent<BackgroundSoundManager>();
		bgsm.musicManager.PlaySong(songName);
	}

	public void SpawnNPC(string npcName, string strX, string strY){
		SceneManager.Instance.npcManager.SpawnNPC(npcName, new Vector3(float.Parse(strX), float.Parse(strY), 0));
	}

	public void StartBlackout(float transitionDuration, float holdDuration){
		cutsceneManager.StartBlackout(transitionDuration, holdDuration);
	}

	public void SetFill(string fillName, string activeStr){
		cutsceneManager.SetFill(fillName, bool.Parse(activeStr));
	}

	public void SetIntegerMarker(string markerName, string valueStr){
		int value = int.Parse(valueStr);
		SessionManager.Instance.SetIntegerMarker(markerName, value);
	}

	public void SetProgressMarker(string markerName, string valueStr){
		bool value = bool.Parse(valueStr);
		SessionManager.Instance.SetData(markerName, value);
	}

	public void ResetBackdrop(){
		SceneManager.Instance.backdropManager.SetBackdrop();
	}

	public void SetNPCDefaultAnimation(string npcName, string animationName){
		SceneManager.Instance.npcManager.SetNPCDefaultAnimation(npcName, animationName);
	}

	public void SetNPCDefaultAnimationDistant(string npcName, string animationName, string roomName){
		SceneManager.Instance.npcManager.SetNPCDefaultAnimation(npcName, animationName, roomName);
	}
}
