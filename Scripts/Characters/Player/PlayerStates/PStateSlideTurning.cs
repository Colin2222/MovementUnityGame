using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateSlideTurning : PState
{
    public PStateSlideTurning(){
		
	}
	
    public override PState Update(){
		base.InverseDirectionCorrection();
		PState.player.animator.Play("PlayerSlideTurning");
		return this;
	}
	
	public override PState FixedUpdate(){
		// apply resistive force
		PState.rigidbody.AddForce(PState.rigidbody.velocity * PState.attr.moveForce * -1.0f * PState.attr.slideForceMultiplier, ForceMode2D.Force);
		
		// check if player has slowed down enough to exit slide
		if(Mathf.Abs(PState.rigidbody.velocity.x) < PState.attr.slideStopSpeedTarget){
			return new PStateIdle();
		}
		return this;
	}
	
    public override PState HitGround(float hitSpeed){
		return this;
	}
	
	public override PState Move(float horizontal, float vertical){
		if(Mathf.Sign(horizontal) == Mathf.Sign(PState.rigidbody.velocity.x) && horizontal != 0.0f){
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
		PState.player.animator.Play("PlayerJumpBracing");
		return new PStateJumpBracing();
	}
	
	public override PState ReleaseJump(){
		return this;
	}
	
	public override PState HitWall(Vector2 wallCollisionVelocity){
		if(Mathf.Abs(wallCollisionVelocity.x) > PState.attr.wallSplatMinSpeed){
			return new PStateWallSplatting((int)Mathf.Sign(wallCollisionVelocity.x));
		}
		
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
