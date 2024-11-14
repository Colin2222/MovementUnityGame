using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateRollEntering : PState
{
	float rollWindowTimer;
	float rollWindow;
	
    public PStateRollEntering(){
		player.animator.Play("PlayerRollEntering");
		player.soundInterface.PlayStillJumpLand();
		rollWindow = attr.groundRollBraceWindow;
		rollWindowTimer = 0.0f;
	}
	
    public override PState Update(){
		rollWindowTimer += Time.deltaTime;
		if(rollWindowTimer >= rollWindow){
			return new PStateLandingBig();
		}
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
		return new PStateRolling(physics.lastBottomCollisionSpeed.x, physics.lastBottomCollisionSpeed.y);
	}
	
	public override PState LeaveGround(){
		player.animator.Play("PlayerSoaringStill");
		return new PStateSoaring();
	}
	
	public override PState LeaveWall(){
		return this;
	}
	
	public override PState ToggleJournal(){
		return this;
	}
}
