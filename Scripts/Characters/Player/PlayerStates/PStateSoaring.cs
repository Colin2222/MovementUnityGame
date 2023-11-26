using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateSoaring : PState
{
    public PStateSoaring(){
		
	}
	
    public override PState Update(){
		return this;
	}
	
	public override PState FixedUpdate(){
		return this;
	}
	
    public override PState HitGround(float hitSpeed){
		PState.player.soundInterface.PlayStillJumpLand();
		return new PStateMoving();
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
	
	public override PState HitWall(Vector2 wallCollisionVelocity){
		if(Mathf.Abs(wallCollisionVelocity.x) > 0.0f){
			return new PStateWallBracing(wallCollisionVelocity);
		} else{
			return this;
		}
	}
	
	public override PState Brace(){
		if(PState.player.cornerHandler.mantleCorner != null){
			return new PStateCornerMantling();
		} else if(PState.player.cornerHandler.corner != null){
			return new PStateCornerGrabbing();
		}
		return this;
	}
	
	public override PState LeaveGround(){
		return this;
	}
	
	public override PState LeaveWall(){
		return this;
	}
}
