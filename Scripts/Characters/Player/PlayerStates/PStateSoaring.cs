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
		lastAirSpeed = rigidbody.velocity.x;
		return this;
	}
	
    public override PState HitGround(float hitSpeedX, float hitSpeedY){
		if(hitSpeedY > attr.groundHitSpeedRollThreshold){
			return new PStateRollEntering();
		} else if(hitSpeedY > attr.groundHitSpeedRollMin && inputManager.bracing){
			return new PStateRolling(hitSpeedX, hitSpeedY);
		}
		timeSinceLastGroundHit = 0.0f;
		lastGroundHitSpeed = hitSpeedY;
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
		if(!collInfo.touchFaceplant && collInfo.touchFeet && Mathf.Abs(wallCollisionVelocity.x) > attr.cornerTripMinimumSpeed){
			return new PStateCornerFaceplanting(wallCollisionVelocity);
		} else if(!collInfo.touchLowerHead && collInfo.touchHead && Mathf.Abs(wallCollisionVelocity.x) > attr.cornerTripMinimumSpeed){
			return new PStateCornerHeadHitting(wallCollisionVelocity);
		}
		
		if(Mathf.Abs(wallCollisionVelocity.x) > 0.0f){
			return new PStateWallBracing(wallCollisionVelocity);
		} else{
			return this;
		}
	}
	
	public override PState Brace(){
		if(player.cornerHandler.mantleCorner != null){
			return new PStateCornerMantling();
		} else if(player.cornerHandler.corner != null){
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

	public override PState Interact(){
		return this;
	}
	
	public override PState ToggleJournal(){
		return this;
	}

	public override PState ToggleInventory(){
		return this;
	}
}
