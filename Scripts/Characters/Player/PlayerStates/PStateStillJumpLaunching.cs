using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateStillJumpLaunching : PState
{
	float jumpLaunchTimer;
	float jumpForceMultiplier;
	float aimAngle;
	
    public PStateStillJumpLaunching(float jumpForceMultiplier, float aimAngle){
		jumpLaunchTimer = 0.0f;
		this.jumpForceMultiplier = jumpForceMultiplier;
		this.aimAngle = aimAngle;
	}
	
    public override PState Update(){
		PState.player.animator.Play("PlayerJumpingStill");
		jumpLaunchTimer += Time.deltaTime;
		
		if(jumpLaunchTimer >= PState.attr.stillJumpLaunchTime){
			PState.rigidbody.velocity = new Vector2(PState.rigidbody.velocity.x, 0);
			
			if(aimAngle == 0.0f){
				aimAngle = 1.5707f;
			}
			if(PState.direction == 1){
				if(aimAngle >= 0){
					if(aimAngle < 1.5707f - PState.attr.maxStillJumpAngleFromYAxis){
						aimAngle = 1.5707f - PState.attr.maxStillJumpAngleFromYAxis;
					} else if (aimAngle > 1.5707f){
						aimAngle = 1.5707f;
					}
				} else{
					if(aimAngle > -.785f){
						aimAngle = 1.5707f - PState.attr.maxStillJumpAngleFromYAxis;
					} else{
						aimAngle = 1.5707f;
					}
				}
			} else if(PState.direction == -1){
				if(aimAngle >= 0){
					if(aimAngle > 1.5707f + PState.attr.maxStillJumpAngleFromYAxis){
						aimAngle = 1.5707f + PState.attr.maxStillJumpAngleFromYAxis;
					} else if(aimAngle < 1.5707f){
						aimAngle = 1.5707f;
					}
				} else{
					if(aimAngle < -2.356f){
						aimAngle = 1.5707f + PState.attr.maxStillJumpAngleFromYAxis;
					} else{
						aimAngle = 1.5707f;
					}
				}
			}
			rigidbody.AddForce(new Vector2(PState.attr.jumpForce * Mathf.Cos(aimAngle) * jumpForceMultiplier, PState.attr.jumpForce * Mathf.Sin(aimAngle) * jumpForceMultiplier), ForceMode2D.Impulse);
			PState.player.physics.ClearBottomCheck();
			return new PStateSoaring();
		}
		return this;
	}
	
	public override PState FixedUpdate(){
		// apply resistive force
		PState.rigidbody.AddForce(PState.rigidbody.velocity * PState.attr.moveForce * -1.0f * PState.attr.slideForceMultiplier, ForceMode2D.Force);
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
		if(PState.player.cornerHandler.mantleCorner != null){
			return new PStateCornerMantling();
		} else if(PState.player.cornerHandler.corner != null){
			return new PStateCornerGrabbing();
		}
		return this;
	}
	
	public override PState LeaveGround(){
		PState.player.animator.Play("PlayerSoaringStill");
		return new PStateSoaring();
	}
	
	public override PState LeaveWall(){
		return this;
	}
	
	public override PState ToggleJournal(){
		return this;
	}
}
