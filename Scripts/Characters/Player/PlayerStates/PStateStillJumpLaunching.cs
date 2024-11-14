using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateStillJumpLaunching : PState
{
	float jumpLaunchTimer;
	float jumpForceMultiplier;
	float aimAngle;
	float jumpMag;
	
    public PStateStillJumpLaunching(float jumpForceMultiplier, float aimAngle, float jumpMag){
		jumpLaunchTimer = 0.0f;
		this.jumpForceMultiplier = jumpForceMultiplier;
		this.aimAngle = aimAngle;
		this.jumpMag = jumpMag;
	}
	
    public override PState Update(){
		player.animator.Play("PlayerJumpingStill");
		jumpLaunchTimer += Time.deltaTime;
		
		if(jumpLaunchTimer >= attr.stillJumpLaunchTime){
			rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
			
			if(aimAngle == 0.0f){
				aimAngle = 1.5707f;
			}
			if(direction == 1){
				if(aimAngle >= 0){
					if(aimAngle < 1.5707f - attr.maxStillJumpAngleFromYAxis){
						aimAngle = 1.5707f - attr.maxStillJumpAngleFromYAxis;
					} else if (aimAngle > 1.5707f){
						aimAngle = 1.5707f;
					}
				} else{
					if(aimAngle > -.785f){
						aimAngle = 1.5707f - attr.maxStillJumpAngleFromYAxis;
					} else{
						aimAngle = 1.5707f;
					}
				}
			} else if(direction == -1){
				if(aimAngle >= 0){
					if(aimAngle > 1.5707f + attr.maxStillJumpAngleFromYAxis){
						aimAngle = 1.5707f + attr.maxStillJumpAngleFromYAxis;
					} else if(aimAngle < 1.5707f){
						aimAngle = 1.5707f;
					}
				} else{
					if(aimAngle < -2.356f){
						aimAngle = 1.5707f + attr.maxStillJumpAngleFromYAxis;
					} else{
						aimAngle = 1.5707f;
					}
				}
			}
			float horizontalForce = attr.jumpForce * Mathf.Cos(aimAngle) * jumpForceMultiplier * jumpMag;
			float verticalForce = attr.jumpForce * Mathf.Sin(aimAngle) * jumpForceMultiplier * jumpMag;
			if(verticalForce < attr.minVerticalJumpForce){
				verticalForce = attr.minVerticalJumpForce;
			}
			rigidbody.AddForce(new Vector2(horizontalForce, verticalForce), ForceMode2D.Impulse);
			player.physics.ClearBottomCheck();
			player.soundInterface.PlayStillJump();
			return new PStateSoaring();
		}
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
		if(player.cornerHandler.mantleCorner != null){
			return new PStateCornerMantling();
		} else if(player.cornerHandler.corner != null){
			return new PStateCornerGrabbing();
		}
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
