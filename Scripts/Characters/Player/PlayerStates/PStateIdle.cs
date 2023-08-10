using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateIdle : PState
{
	public PStateIdle(PlayerHub player, PlayerAttributeSet attr, Rigidbody2D rigidbody){
		PState.player = player;
		PState.attr = attr;
		PState.rigidbody = rigidbody;
		PState.direction = 1;
	}
	
	public PStateIdle(){
		
	}
	
    public override PState Update(){
		PState.player.animator.Play("PlayerIdle");
		return this;
	}
	
	public override PState FixedUpdate(){
		// apply resistive force to prevent some residual sliding after ending a slide stop/turn
		PState.rigidbody.AddForce(PState.rigidbody.velocity * PState.attr.moveForce * -1.0f, ForceMode2D.Force);
		return this;
	}
	
    public override PState HitGround(){
		return this;
	}
	
	public override PState Move(float horizontal, float vertical){
		if(Mathf.Abs(horizontal) > 0.0f){
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
