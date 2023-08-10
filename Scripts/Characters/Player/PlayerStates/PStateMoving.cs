using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateMoving : PState
{
	float horizontal;
	float vertical;
	
    public PStateMoving(){
		
	}
	
    public override PState Update(){
		PState.player.animator.Play("PlayerRunning");
		return this;
	}
	
	public override PState FixedUpdate(){
		// apply running force
		PState.rigidbody.AddForce(new Vector2(horizontal, 0.0f) * PState.attr.moveForce, ForceMode2D.Force);
		
		// hold player under maximum run speed
		if(Mathf.Abs(PState.rigidbody.velocity.x) > PState.attr.maxRunSpeed){
			PState.rigidbody.velocity = new Vector2(PState.attr.maxRunSpeed * Mathf.Sign(PState.rigidbody.velocity.x), PState.rigidbody.velocity.y);
		}
		return this;
	}
	
    public override PState HitGround(){
		return this;
	}
	
	public override PState Move(float horizontal, float vertical){
		this.horizontal = horizontal;
		this.vertical = vertical;
		
		if(horizontal == 0.0f){
			base.SetDirection(horizontal);
			return new PStateSlideStopping();
		} else if(Mathf.Sign(horizontal) != Mathf.Sign(PState.rigidbody.velocity.x) && Mathf.Abs(PState.rigidbody.velocity.x) > PState.attr.slideStopSpeedTarget){
			base.SetDirection(horizontal);
			return new PStateSlideTurning();
		} else{
			base.SetDirection(horizontal);
			
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
