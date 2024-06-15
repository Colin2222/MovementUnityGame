using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateLandingBig : PState
{
	float landingTimer;
	float landingTime;
	float moveForce;
	float landingBigForceMultiplier;
	
    public PStateLandingBig(){
		PState.player.animator.Play("PlayerLandingBig");
		landingTime = PState.attr.landingBigTime;
		moveForce = PState.attr.moveForce;
		landingBigForceMultiplier = PState.attr.landingBigForceMultiplier;
	}
	
    public override PState Update(){
		landingTimer += Time.deltaTime;
		if(landingTimer >= landingTime){
			return new PStateIdle();
		}
		return this;
	}
	
	public override PState FixedUpdate(){
		// apply resistive force
		PState.rigidbody.AddForce(PState.rigidbody.velocity * moveForce * -1.0f * landingBigForceMultiplier, ForceMode2D.Force);
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
	
	public override PState LeaveGround(){
		return new PStateSoaring();
	}
	
	public override PState LeaveWall(){
		return this;
	}
	
	public override PState ToggleJournal(){
		return this;
	}
}
