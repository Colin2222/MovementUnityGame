using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PStateInteracting : PState, IMenuState
{
    PlayerInventoryHandler inventoryHandler;
    Interactable interactable;
	Site site;

	GameObject cameraTargetObj;
	float originalCameraDistance;
	
	public PStateInteracting(Interactable interactable){
		inventoryHandler = player.inventoryHandler;
		this.interactable = interactable;
	}
	
    public override PState Update(){
		return this;
	}
	
	public override PState FixedUpdate(){
		return this;
	}
	
    public override PState HitGround(float hitSpeedX, float hitSpeedY){
		return this;
	}
	
	public override PState Move(float horizontal, float vertical){
		return this;
	}
	
	public override PState ClimbUp(){
		return this;
	}
	
	public override PState ClimbDown(){
		return this;
	}
	
	public override PState PressJump(){
		return this;
	}
	
	public override PState ReleaseJump(){
		return this;
	}
	
	public override PState HitWall(Vector2 wallCollisionVelocity, WallCollisionInfo collInfo){
		return this;
	}
	
	public override PState Brace(){
		return this;
	}

	public override PState Grab(){
		return this;
	}
	
	public override PState LeaveGround(){
		return this;
	}
	
	public override PState LeaveWall(){
		return this;
	}

    public override PState Interact(){
		return this;
	}
	
	public override PState ToggleJournal(){
		return this;
	}

	public override PState ToggleInventory(){
		return this;
	}

	public void MenuUp(){
		interactable.MenuUp();
	}

	public void MenuDown(){
		interactable.MenuDown();
	}

	public void MenuLeft(){
		interactable.MenuLeft();
	}

	public void MenuRight(){
		interactable.MenuRight();
	}

	public void MenuSelect(){
		interactable.MenuSelect();
	}

	public void MenuDrop(){
		
	}

	public void MenuInteract(){
		interactable.MenuInteract();
	}

	public PState MenuExit(){
        interactable.LeaveInteraction();

		return new PStateIdle();
	}
}
