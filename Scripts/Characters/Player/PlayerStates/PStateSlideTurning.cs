using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateSlideTurning : PState
{
    public PStateSlideTurning(){
		
	}
	
    public override PState Update(){
		base.InverseDirectionCorrection();
		player.animator.Play("PlayerSlideTurning");
		return this;
	}
	
	public override PState FixedUpdate(){
		// apply resistive force
		rigidbody.AddForce(rigidbody.velocity * attr.moveForce * -1.0f * attr.slideForceMultiplier, ForceMode2D.Force);
		
		// check if player has slowed down enough to exit slide
		if(Mathf.Abs(rigidbody.velocity.x) < attr.slideStopSpeedTarget){
			return new PStateIdle();
		}
		return this;
	}
	
    public override PState HitGround(float hitSpeedX, float hitSpeedY){
		return this;
	}
	
	public override PState Move(float horizontal, float vertical){
		if(Mathf.Sign(horizontal) == Mathf.Sign(rigidbody.velocity.x) && horizontal != 0.0f){
			return new PStateMoving();
		}
		return this;
	}
	
	public override PState ClimbUp(){
		return this;
	}
	
	public override PState ClimbDown(){
		return this;
	}
	
	public override PState PressJump(){
		player.animator.Play("PlayerJumpBracing");
		return new PStateJumpBracing();
	}
	
	public override PState ReleaseJump(){
		return this;
	}
	
	public override PState HitWall(Vector2 wallCollisionVelocity, WallCollisionInfo collInfo){
		if(Mathf.Abs(wallCollisionVelocity.x) > attr.wallSplatMinSpeed){
			return new PStateWallSplatting((int)Mathf.Sign(wallCollisionVelocity.x));
		}
		
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
