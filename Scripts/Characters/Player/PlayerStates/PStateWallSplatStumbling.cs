using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateWallSplatStumbling : PState
{
	int splatDirection;
	float wallSplatStumbleTimer;
	
    public PStateWallSplatStumbling(int direction){
		PState.player.animator.Play("PlayerWallSplatStumbling");
		splatDirection = direction;
		wallSplatStumbleTimer = PState.attr.wallSplatStumbleTime;
		PState.rigidbody.velocity = new Vector2(PState.attr.wallSplatStumbleSpeed * splatDirection, PState.rigidbody.velocity.y);
	}
	
    public override PState Update(){
		wallSplatStumbleTimer -= Time.deltaTime;
		if(wallSplatStumbleTimer <= 0.0f){
			return new PStateIdle();
		}
		return this;
	}
	
	public override PState FixedUpdate(){
		return this;
	}
	
    public override PState HitGround(float hitSpeed){
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
	
	public override PState HitWall(Vector2 wallCollisionVelocity){
		return this;
	}
	
	public override PState Brace(){
		return this;
	}
	
	public override PState LeaveGround(){
		return this;
	}
	
	public override PState LeaveWall(){
		return this;
	}
}
