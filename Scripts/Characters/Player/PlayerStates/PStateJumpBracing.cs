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
		rigidbody.AddForce(rigidbody.velocity * attr.moveForce * -1.0f * attr.slideForceMultiplier, ForceMode2D.Force);
		return this;
	}
	
    public override PState HitGround(float hitSpeedX, float hitSpeedY){
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
		float jumpForceMultiplier = Mathf.Clamp(jumpBraceCounter, 0.0f, attr.jumpBraceTime) / attr.jumpBraceTime;
		if(jumpForceMultiplier > attr.stillJumpMinimumBraceRatio){
			float aimAngle = Mathf.Atan2(vertical, horizontal);
			float jumpMag = new Vector2(horizontal, vertical).magnitude;
			// TO DO ADD CONTROLLER DEADZONE
			if(jumpMag < 0.04f){
				jumpMag = 1.0f;
			}
			return new PStateStillJumpLaunching(jumpForceMultiplier, aimAngle, jumpMag);
		}
		return new PStateIdle();
	}
	
	public override PState HitWall(Vector2 wallCollisionVelocity, WallCollisionInfo collInfo){
		return this;
	}
	
	public override PState Brace(){
		return this;
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
