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
		PState.lastAirSpeed = PState.rigidbody.velocity.x;
		return this;
	}
	
    public override PState HitGround(float hitSpeedX, float hitSpeedY){
		if(hitSpeedY > PState.attr.groundHitSpeedRollThreshold){
			return new PStateRollEntering();
		} else if(hitSpeedY > PState.attr.groundHitSpeedRollMin && PState.inputManager.bracing){
			return new PStateRolling(hitSpeedX, hitSpeedY);
		}
		PState.timeSinceLastGroundHit = 0.0f;
		PState.lastGroundHitSpeed = hitSpeedY;
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
	
	public override PState HitWall(Vector2 wallCollisionVelocity, WallCollisionInfo collInfo){
		if(!collInfo.touchLowerMiddle && collInfo.touchFeet && Mathf.Abs(wallCollisionVelocity.x) > PState.attr.cornerTripMinimumSpeed){
			//return new PStateCornerFaceplanting(wallCollisionVelocity);
		}
		
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
	
	public override PState ToggleJournal(){
		return this;
	}
}
