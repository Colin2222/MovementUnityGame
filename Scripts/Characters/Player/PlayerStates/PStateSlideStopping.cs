using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateSlideStopping : PState
{
    public PStateSlideStopping(){
		
	}
	
    public override PState Update(){
		PState.player.animator.Play("PlayerSlideStopping");
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
	
    public override PState HitGround(){
		return this;
	}
	
	public override PState Move(float horizontal, float vertical){
		if(Mathf.Sign(horizontal) != Mathf.Sign(PState.rigidbody.velocity.x) && horizontal != 0.0f){
			return new PStateSlideTurning();
		} else if(Mathf.Sign(horizontal) == Mathf.Sign(PState.rigidbody.velocity.x) && horizontal != 0.0f){
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
		return this;
	}
	
	public override PState ReleaseJump(){
		return this;
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
