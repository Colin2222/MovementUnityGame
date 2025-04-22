using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateViewingJournal : PState
{
	JournalManager journalManager;
	
	public PStateViewingJournal(){
		journalManager = GameObject.FindWithTag("SceneManager").GetComponent<SceneManager>().journalManager;
		journalManager.Activate();
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
		journalManager.Deactivate();
		return new PStateIdle();
	}

	public override PState ToggleInventory(){
		return this;
	}
}
