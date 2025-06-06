using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateViewingInventory : PState, IMenuState
{
    PlayerInventoryHandler inventoryHandler;
	
	public PStateViewingInventory(){
		inventoryHandler = player.inventoryHandler;
		inventoryHandler.ToggleInventory();
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
		inventoryHandler.ToggleInventory();
		return new PStateIdle();
	}

	public void MenuUp(){
		inventoryHandler.MoveUp();
	}

	public void MenuDown(){
		inventoryHandler.MoveDown();
	}

	public void MenuLeft(){
		inventoryHandler.MoveLeft();
	}

	public void MenuRight(){
		inventoryHandler.MoveRight();
	}

	public void MenuSelect(){
		inventoryHandler.MenuSelect();
	}

	public void MenuDrop(){
		inventoryHandler.DropCurrentItem();
	}

	public void MenuInteract(){
		
	}

	public PState MenuExit(){
		inventoryHandler.ToggleInventory();
		return new PStateIdle();
	}
}
