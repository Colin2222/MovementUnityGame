using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
	public PlayerHub player;
    PState currentState;
	bool settingTransformFollow = false;
	
	public Type GetStateType(){
		return currentState.GetType();
	}

	public bool IsMenuState(){
		return (currentState is IMenuState);
	}
	
	void Start(){
		currentState = new PStateIdle(player, player.inputManager, player.attributeManager.attrSet, player.rigidbody, player.physics, player.interactor);
	}
	
	void Update(){
		currentState = currentState.Update();
	}
	
	void FixedUpdate(){
		currentState = currentState.FixedUpdate();
	}

	public void ResetPlayer(){
		currentState = new PStateIdle();
	}
	public void ResetPlayerNoAnim(){
		currentState = new PStateIdle(false);
	}

	public void Move(float horizontal, float vertical)
	{
		currentState = currentState.Move(horizontal, vertical);
	}
	
	public void HitGround(float hitSpeedX, float hitSpeedY, bool stayCollision){
		// add an option for the weird stay collisions, only triggers in the states where it gets weird
		if(stayCollision){
			if(currentState is PStateFaceplantSoaring || currentState is PStateHeadHitSoaring){
				currentState = currentState.HitGround(hitSpeedX, hitSpeedY);
			}
		} else{
			currentState = currentState.HitGround(hitSpeedX, hitSpeedY);
		}
	}
	
	public void HitWall(Vector2 wallCollisionVelocity, WallCollisionInfo collInfo){
		currentState = currentState.HitWall(wallCollisionVelocity, collInfo);
	}
	
	public void LeaveGround(){
		if(!settingTransformFollow){
			currentState = currentState.LeaveGround();
		}
	}
	
	public void LeaveWall(){
		currentState = currentState.LeaveWall();
	}
	
	public void PressJump(){
		currentState = currentState.PressJump();
	}
	
	public void ReleaseJump(){
		currentState = currentState.ReleaseJump();
	}
	
	public bool Brace(){
		PState oldState = currentState;
		currentState = currentState.Brace();
		return oldState != currentState;
	}

	public bool Grab(){
		PState oldState = currentState;
		currentState = currentState.Grab();
		return oldState != currentState;
	}
	
	public void ClimbUp(){
		currentState = currentState.ClimbUp();
	}
	
	public void ClimbDown(){
		currentState = currentState.ClimbDown();
	}

	// returns true if the player successfully interacts with an interactable that halts movement (menus, dialogue, etc.)
	public bool Interact(){
		currentState = currentState.Interact();
		return (currentState is PStateInteracting);
	}
	
	public bool ToggleJournal(){
		currentState = currentState.ToggleJournal();
		return (currentState is IMenuState);
	}

	public bool ToggleInventory(){
		currentState = currentState.ToggleInventory();
		return (currentState is IMenuState);
	}

	public void MenuUp(){
		if(currentState is IMenuState){
			((IMenuState)currentState).MenuUp();
		}
	}

	public void MenuDown(){
		if(currentState is IMenuState){
			((IMenuState)currentState).MenuDown();
		}
	}

	public void MenuLeft(){
		if(currentState is IMenuState){
			((IMenuState)currentState).MenuLeft();
		}
	}

	public void MenuRight(){
		if(currentState is IMenuState){
			((IMenuState)currentState).MenuRight();
		}
	}

	public void MenuSelect(){
		if(currentState is IMenuState){
			((IMenuState)currentState).MenuSelect();
		}
	}

	public void MenuDrop(){
		if(currentState is IMenuState){
			((IMenuState)currentState).MenuDrop();
		}
	}

	public void MenuInteract(){
		if(currentState is IMenuState){
			((IMenuState)currentState).MenuInteract();
		}
	}

	// returns true if the menu is exited successfully
	public bool MenuExit(){
		if(currentState is IMenuState){
			currentState = ((IMenuState)currentState).MenuExit();
		}
		return (currentState is not IMenuState);
	}

	public void EnterTransformFollow(Transform followTarget){
		if(currentState is PStateIdle){
			settingTransformFollow = true;
			currentState = new PStateTransformFollowing(followTarget);
		}
	}

	public void ExitTransformFollow(){
		if(currentState is PStateTransformFollowing){
			settingTransformFollow = false;
			player.rigidbody.bodyType = RigidbodyType2D.Dynamic;
			player.transform.SetParent(null, true);
			player.physics.gameObject.SetActive(true);
			ResetPlayer();
		}
	}

	public void EnterCutsceneDialogue(Interactable interactable){
		currentState = new PStateInteracting(interactable);
	}

	public void SetDirection(float horizontal){
		currentState.SetDirection(horizontal);
	}

	public int GetDirection(){
		return currentState.GetDirection();
	}
}
