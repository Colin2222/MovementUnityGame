using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
	Vector3 targetPosition;
	bool isFreeMoving = false;
	float freeMovingSpeed = 0.0f;
	Transform baseTransform;

	// Start is called before the first frame update
	void Start()
	{
		cutsceneVelocity = Vector2.zero;

		if (cutsceneManager == null)
		{
			cutsceneManager = GameObject.FindWithTag("SceneManager").GetComponent<SceneManager>().cutsceneManager;
		}
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (cutsceneManager.inCutscene && (cutsceneManager.playerLocked || !isPlayer) && rb != null)
		{
			if (movingToTarget)
			{
				if ((targetDirection == 1 && rb.transform.position.x > targetX) || (targetDirection == -1 && rb.transform.position.x < targetX))
				{
					cutsceneVelocity = new Vector2(0.0f, cutsceneVelocity.y);
					animate(defaultAnim);
					movingToTarget = false;
				}
			}
			rb.velocity = cutsceneVelocity;

			if (isFreeMoving)
			{
				baseTransform.position = Vector3.MoveTowards(baseTransform.position, targetPosition, freeMovingSpeed * Time.deltaTime);
				if (Vector3.Distance(baseTransform.position, targetPosition) < 0.1f)
				{
					isFreeMoving = false;
					freeMovingSpeed = 0.0f;
				}
			}
		}
	}

	public void animate(string animation)
	{
		// TRY AND IMPLEMENT A TRY CATCH HERE, need to figure out type of error when invalid animation string given
		animator.Play(animation);
	}

	public void SetHorizontalVelocity(float velocity)
	{
		cutsceneVelocity = new Vector2(velocity, cutsceneVelocity.y);
	}

	public void SetHorizontalVelocityToTarget(float velocity, float targetX)
	{
		if (targetX > rb.transform.position.x)
		{
			targetDirection = 1;
		}
		else
		{
			targetDirection = -1;
		}

		this.targetX = targetX;
		movingToTarget = true;
		cutsceneVelocity = new Vector2(velocity, cutsceneVelocity.y);
	}

	public void SetVelocityToTarget(float speed, float targetX, float targetY)
	{
		targetPosition = new Vector3(targetX, targetY, rb.transform.position.z);
		freeMovingSpeed = speed;
		isFreeMoving = true;
		if (isPlayer)
		{
			baseTransform = transform.parent;
		}
		else
		{
			baseTransform = transform;
		}

	}

	public void VerticalShift(float shift)
	{
		Transform trans = rb.gameObject.transform;
		trans.position = new Vector3(trans.position.x, trans.position.y + shift, trans.position.z);
	}

	public void SetCoordinates(float x, float y)
	{
		Transform trans = rb.gameObject.transform;
		trans.position = new Vector3(x, y, trans.position.z);
	}

	public void FaceLeft()
	{
		Transform trans = rb.gameObject.transform;
		trans.eulerAngles = new Vector2(0, 180);
	}

	public void FaceRight()
	{
		Transform trans = rb.gameObject.transform;
		trans.eulerAngles = new Vector2(0, 0);
	}

	public void ToggleActivation()
	{
		gameObject.SetActive(!(gameObject.activeInHierarchy));
	}

	public void ToggleGravity()
	{
		if (gravityPaused)
		{
			rb.gravityScale = tempGrav;
			gravityPaused = false;
		}
		else
		{
			tempGrav = rb.gravityScale;
			rb.gravityScale = 0.0f;
			gravityPaused = true;
		}
	}

	public void SetSpriteOrder(float orderF)
	{
		int order = (int)orderF;
		spriteRenderer.sortingOrder = order;
	}

	public void SetCameraAnchorPoint()
	{
		cutsceneManager.SwitchCameraAnchor(gameObject.transform);
	}

	public void SetCameraZoom(float zoom)
	{
		SceneManager.Instance.vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = zoom;
	}

	public void SetCameraDampingX(float damping)
	{
		SceneManager.Instance.vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = damping;
	}

	public void SetCameraDampingY(float damping)
	{
		SceneManager.Instance.vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = damping;
	}

	public void SetCameraDampingZ(float damping)
	{
		SceneManager.Instance.vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ZDamping = damping;
	}

	// only available for one-per-scene DialogueMananger, cutscene actor -1
	public void StartDialogue(string DialogueCode)
	{
		this.transform.parent.GetComponent<DialogueManager>().StartDialogue(DialogueCode);
	}

	public void PlaySong(string songName)
	{
		GameObject bgsmObj = GameObject.FindWithTag("BackgroundSoundManager");
		if (bgsmObj == null) return;
		BackgroundSoundManager bgsm = bgsmObj.GetComponent<BackgroundSoundManager>();
		bgsm.musicManager.PlaySong(songName);
	}

	public void SpawnNPC(string npcName, string strX, string strY)
	{
		SceneManager.Instance.npcManager.SpawnNPC(npcName, new Vector3(float.Parse(strX), float.Parse(strY), 0));
	}

	public void StartBlackout(float transitionDuration, float exitDuration, float holdDuration)
	{
		cutsceneManager.StartBlackout(transitionDuration, exitDuration, holdDuration);
	}

	public void SetFill(string fillName, string activeStr)
	{
		cutsceneManager.SetFill(fillName, bool.Parse(activeStr));
	}

	public void SetFillIsActive(string fillName, string activeStr, string roomName)
	{
		SessionManager.Instance.SetFillIsActive(fillName, bool.Parse(activeStr), roomName);
	}

	public void SetIntegerMarker(string markerName, string valueStr)
	{
		int value = int.Parse(valueStr);
		SessionManager.Instance.SetIntegerMarker(markerName, value);
	}

	public void SetProgressMarker(string markerName, string valueStr)
	{
		bool value = bool.Parse(valueStr);
		SessionManager.Instance.SetData(markerName, value);
	}

	public void ResetBackdrop()
	{
		SceneManager.Instance.backdropManager.SetBackdrop();
	}

	public void SetNPCDefaultAnimation(string npcName, string animationName)
	{
		SceneManager.Instance.npcManager.SetNPCDefaultAnimation(npcName, animationName);
	}

	public void SetNPCDefaultAnimationDistant(string npcName, string animationName, string roomName)
	{
		SceneManager.Instance.npcManager.SetNPCDefaultAnimation(npcName, animationName, roomName);
	}

	public void SetNPCPosition(string npcName, string xStr, string yStr, string roomName = null)
	{
		if (roomName == null)
		{
			SceneManager.Instance.npcManager.SetNPCPosition(npcName, new Vector2(float.Parse(xStr), float.Parse(yStr)));
		}
		else
		{
			SceneManager.Instance.npcManager.SetNPCPosition(npcName, new Vector2(float.Parse(xStr), float.Parse(yStr)), roomName);
		}
	}

	// only used for non-locked cutscenes where player can move around while NPCs are moving
	// makes sure NPCs are saved at the end of their cutscene override
	public void SetNPCDefaultPosition(string npcName, string xStr, string yStr)
	{
		SceneManager.Instance.npcManager.SetDefaultNPCPosition(npcName, new Vector2(float.Parse(xStr), float.Parse(yStr)));
	}

	public void SetNPCDirection(string npcName, string directionStr, string roomName = null)
	{
		if (roomName == null)
		{
			SceneManager.Instance.npcManager.SetNPCDirection(npcName, int.Parse(directionStr));
		}
		else
		{
			SceneManager.Instance.npcManager.SetNPCDirection(npcName, int.Parse(directionStr), roomName);
		}
	}

	public void AddWorldItem(string itemId, string xStr, string yStr, string roomName)
	{
		SceneManager.Instance.itemManager.AddWorldItem(itemId, new Vector2(float.Parse(xStr), float.Parse(yStr)), roomName);
	}

	public void ConstructSite(string siteId)
	{
		SceneManager.Instance.siteManager.ConstructSite(int.Parse(siteId));
	}

	public void SetSiteAdditionalData(string siteId, string dataKey, string dataValue, string roomName)
	{
		SessionManager.Instance.SetSiteAdditionalData(int.Parse(siteId), dataKey, dataValue, roomName);
	}

	public void AnchorCheckBreakpoint(string anchorId, string roomName)
	{
		string siteName = SessionManager.Instance.GetSiteName(int.Parse(anchorId), roomName);
		if (siteName == null || siteName != "cablecar")
		{
			cutsceneManager.EndCutscene();
		}
	}

	public void SetCameraPosition(float x, float y)
	{
		SceneManager.Instance.mainCameraObj.transform.position = new Vector3(x, y, SceneManager.Instance.mainCameraObj.transform.position.z);
		SceneManager.Instance.vcam.transform.position = new Vector3(x, y, SceneManager.Instance.vcam.transform.position.z);
	}

	public void ActivateVcam()
	{
		SceneManager.Instance.vcam.gameObject.SetActive(true);
	}

	public void DeactivateVcam()
	{
		SceneManager.Instance.vcam.gameObject.SetActive(false);
	}

	public void SwitchSceneInCutscene(string sceneIndexStr, string cutsceneName)
	{
		int sceneIndex = int.Parse(sceneIndexStr);
		cutsceneManager.SwitchSceneInCutscene(sceneIndex, cutsceneName);
	}
}
