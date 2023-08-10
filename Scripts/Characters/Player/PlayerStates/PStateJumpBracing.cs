using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateJumpBracing : PState
{
	float jumpBraceCounter;
	float horizontal;
	float vertical;
	
    public PStateJumpBracing(){
		jumpBraceCounter = 0.0f;
	}
	
    public override PState Update(){
		jumpBraceCounter += Time.deltaTime;
		return this;
	}
	
	public override PState FixedUpdate(){
		// apply resistive force
		PState.rigidbody.AddForce(PState.rigidbody.velocity * PState.attr.moveForce * -1.0f * PState.attr.slideForceMultiplier, ForceMode2D.Force);
		return this;
	}
	
    public override PState HitGround(float hitSpeed){
		return this;
	}
	
	public override PState Move(float horizontal, float vertical){
		this.horizontal = horizontal;
		this.vertical = vertical;
		
		base.SetDirection(horizontal);
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
		float jumpForceMultiplier = Mathf.Clamp(jumpBraceCounter, 0.0f, PState.attr.jumpBraceTime) / PState.attr.jumpBraceTime;
		float aimAngle = Mathf.Atan2(vertical, horizontal);
		return new PStateStillJumpLaunching(jumpForceMultiplier, aimAngle);
	}
	
	public override PState HitWall(){
		return this;
	}
	
	public override PState PressBrace(){
		return this;
	}
	
	public override PState LeaveGround(){
		return this;
	}
}
