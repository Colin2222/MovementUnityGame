using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
	public PlayerHub player;
	public PlayerStateManager stateManager;
	public PlayerAttributeSet attr;
	public PlayerInteractor interactor;
	
	// locking player input
	bool locked = false;

	// player's actions overridden (not cutscene)
	bool overridden = false;
	
	// in ui
	[System.NonSerialized]
	public bool inUI = false;

	// in cutscene
	bool inCutscene = false;
	
	// joystick movement
	float horizontal;
	float vertical;
	
	// bracing tracking
	public bool bracing = false;
	float braceTimer;
	float braceCooldownTimer;
	bool braceCooldownCancelled = false;
	
	// climbing up/down tracking
	bool canClimbUp = false;
	bool canClimbDown = false;
	
	// button pressing
    private bool jumpPressed = false;
    private bool jumpJustPressed = false;
	private bool bracePressed = false;
	private bool braceJustPressed = false;
	private bool inventoryPressed = false;
	private bool inventoryJustPressed = false;
	private bool itemGrabPressed = false;
	private bool itemGrabJustPressed = false;
	private bool menuUpPressed = false;
	private bool menuUpJustPressed = false;
	private bool menuDownPressed = false;
	private bool menuDownJustPressed = false;
	private bool menuRightPressed = false;
	private bool menuRightJustPressed = false;
	private bool menuLeftPressed = false;
	private bool menuLeftJustPressed = false;
	private bool menuSelectPressed = false;
	private bool menuSelectJustPressed = false;
	private bool menuDropPressed = false;
	private bool menuDropJustPressed = false;
	private bool menuExitPressed = false;
	private bool menuExitJustPressed = false;
	private bool menuPageRightPressed = false;
	private bool menuPageRightJustPressed = false;
	private bool menuPageLeftPressed = false;
	private bool menuPageLeftJustPressed = false;
	private bool interactPressed = false;
	private bool interactJustPressed = false;
	private bool toggleJournalPressed = false;
	private bool toggleJournalJustPressed = false;
	private bool toggleInventoryPressed = false;
	private bool toggleInventoryJustPressed = false;
	
    // Start is called before the first frame update
    void Start()
    {
        attr = player.attributeManager.attrSet;
    }

    // Update is called once per frame
    void Update()
    {
		if(overridden){
			stateManager.Move(0, 0);
		}
		if(!locked){
			stateManager.Move(horizontal, vertical);
			HandleBracing();
			HandleClimbing();
			HandleItemGrabbing();
			
			if(bracing){
				bracing = !(stateManager.Brace());
			}
		} else if(inUI){
			HandleMenu();
		} else if(inCutscene){
			
		}
		
        jumpJustPressed = false;
		braceJustPressed = false;
		inventoryJustPressed = false;
		itemGrabJustPressed = false;
		menuUpJustPressed = false;
		menuDownJustPressed = false;
		menuRightJustPressed = false;
		menuLeftJustPressed = false;
		menuSelectJustPressed = false;
		menuDropJustPressed = false;
		menuExitJustPressed = false;
		menuPageRightJustPressed = false;
		menuPageLeftJustPressed = false;
		interactJustPressed = false;
		toggleJournalJustPressed = false;
		toggleInventoryJustPressed = false;
    }
	
	public void LockPlayer(){
		locked = true;
	}
	
	public void UnlockPlayer(){
		locked = false;
		overridden = false;
	}

	public void OverridePlayer(){
		overridden = true;
	}

	public void EndOverridePlayer(){
		overridden = false;
	}

	public void EnterCutscene(){
		inCutscene = true;
	}

	public void ExitCutscene(){
		inCutscene = false;
	}

	public void LeaveInteraction(){
		if(stateManager.MenuExit()){
			UnlockPlayer();
			inUI = false;
		}
	}

	// has to just reset player to idle state to prevent loop between stateManager MenuExit and NPCInteractable LeaveInteraction and DialogueManager EndDialogue 
	// shits fucked
	public void LeaveDialogue(){
		stateManager.ResetPlayerNoAnim();
		locked = false;
		inUI = false;
	}
	
	private void HandleMenu(){
		if(inUI){
			if(menuUpJustPressed){
				stateManager.MenuUp();
			}
			if(menuDownJustPressed){
				stateManager.MenuDown();
			}
			if(menuRightJustPressed){
				stateManager.MenuRight();
			}
			if(menuLeftJustPressed){
				stateManager.MenuLeft();
			}
			if(menuSelectJustPressed){
				stateManager.MenuSelect();
			}
			if(menuDropJustPressed){
				stateManager.MenuDrop();
			}
			if(menuExitJustPressed){
				if(stateManager.MenuExit()){
					UnlockPlayer();
					inUI = false;
				}
			}
		}
	}

	// handles bracing timing
	private void HandleBracing(){
		if(bracing){
			braceTimer -= Time.deltaTime;
			if(braceTimer <= 0.0f || !bracePressed){
				bracing = false;
				if(braceCooldownCancelled){
					braceCooldownCancelled = false;
				} else{
					braceCooldownTimer = attr.braceCooldownTime;
				}
			}
		} else{
			if(braceCooldownTimer > 0.0f){
				braceCooldownTimer -= Time.deltaTime;
			} else{
				if(braceJustPressed){
					bracing = true;
					braceTimer = attr.braceTime;
				}
			}
		}
	}

	private void HandleItemGrabbing(){
		if(itemGrabJustPressed){
			player.inventoryHandler.Pickup();
		}
	}
	
	public void CancelBraceCooldown(){
		braceCooldownCancelled = true;
	}
	
	private void HandleClimbing(){
		if(vertical <= -(attr.cornerClimbVertJoystickThreshold)){
			canClimbUp = true;
			if(canClimbDown){
				stateManager.ClimbDown();
				canClimbDown = false;
			}
		} else if(vertical >= attr.cornerClimbVertJoystickThreshold){
			canClimbDown = true;
			if(canClimbUp){
				stateManager.ClimbUp();
				canClimbUp = false;
			}
		} else {
			canClimbDown = true;
			canClimbUp = true;
		}
	}
	
	// controller button methods
    private void OnMove(InputValue value){
        Vector2 vector = value.Get<Vector2>();

        horizontal = vector.x;
        vertical = vector.y;
    }
	
	private void OnJump(){
        jumpPressed = !jumpPressed;
        jumpJustPressed = jumpPressed;
		
		if(!locked){
			if(jumpPressed){
				stateManager.PressJump();
			} else{
				stateManager.ReleaseJump();
			}
		}
    }
	
	private void OnBrace(){
        bracePressed = !bracePressed;
        braceJustPressed = bracePressed;
    }
	
	private void OnInventory(){
		inventoryPressed = !inventoryPressed;
		inventoryJustPressed = inventoryPressed;
	}
	
	private void OnItemGrab(){
		itemGrabPressed = !itemGrabPressed;
		itemGrabJustPressed = itemGrabPressed;
	}
	
	private void OnMenuUp(){
		menuUpPressed = !menuUpPressed;
		menuUpJustPressed = menuUpPressed;
	}
	
	private void OnMenuDown(){
		menuDownPressed = !menuDownPressed;
		menuDownJustPressed = menuDownPressed;
	}
	
	private void OnMenuRight(){
		menuRightPressed = !menuRightPressed;
		menuRightJustPressed = menuRightPressed;
	}
	
	private void OnMenuLeft(){
		menuLeftPressed = !menuLeftPressed;
		menuLeftJustPressed = menuLeftPressed;
	}

	private void OnMenuExit(){
		menuExitPressed = !menuExitPressed;
		menuExitJustPressed = menuExitPressed;
	}

	private void OnMenuSelect(){
		menuSelectPressed = !menuSelectPressed;
		menuSelectJustPressed = menuSelectPressed;
	}

	private void OnMenuDrop(){
		menuDropPressed = !menuDropPressed;
		menuDropJustPressed = menuDropPressed;
	}
	
	private void OnMenuPageRight(){
		menuPageRightPressed = !menuPageRightPressed;
		menuPageRightJustPressed = menuPageRightPressed;
	}
	
	private void OnMenuPageLeft(){
		menuPageLeftPressed = !menuPageLeftPressed;
		menuPageLeftJustPressed = menuPageLeftPressed;
	}
	
	private void OnInteract(){
		interactPressed = !interactPressed;
		interactJustPressed = interactPressed;
		
		if(interactJustPressed){
			if(!locked && interactJustPressed){
				if(stateManager.Interact()){
					LockPlayer();
					inUI = true;
				}
			} else if(stateManager.IsMenuState()){
				stateManager.MenuInteract();
			}
		}
	}
	
	private void OnToggleJournal(){
		toggleJournalPressed = !toggleJournalPressed;
		toggleJournalJustPressed = toggleJournalPressed;
		
		if(toggleJournalJustPressed){
			if(stateManager.ToggleJournal()){
				LockPlayer();
				inUI = true;
			} else{
				UnlockPlayer();
				inUI = false;
			}
		}
	}

	private void OnToggleInventory(){
		toggleInventoryPressed = !toggleInventoryPressed;
		toggleInventoryJustPressed = toggleInventoryPressed;
		
		if(toggleInventoryJustPressed){
			if(!inCutscene && stateManager.ToggleInventory()){
				LockPlayer();
				inUI = true;
			} else if(inUI){
				UnlockPlayer();
				inUI = false;
			}
		}
	}
	
	private void OnSaveData(){
		GameObject.FindWithTag("SessionManager").GetComponent<SessionManager>().SetCurrentRoom();
		GameObject.FindWithTag("SessionManager").GetComponent<SessionManager>().SetRoomItems();
		GameObject.FindWithTag("SessionManager").GetComponent<SessionManager>().SetPlayerInventory(player.inventoryHandler.inventory.SaveInventory());
		GameObject.FindWithTag("SessionManager").GetComponent<SessionManager>().SaveData();
		Debug.Log("DATA SAVED");
	}
}
