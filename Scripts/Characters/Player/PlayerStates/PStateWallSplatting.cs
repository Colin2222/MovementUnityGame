using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateWallSplatting : PState
{
	float wallSplatStickTimer;
	int splatDirection;
	
    public PStateWallSplatting(int direction){
		PState.player.soundInterface.PlayWallImpact();
		PState.player.animator.Play("PlayerWallSplatting");
		wallSplatStickTimer = PState.attr.wallSplatStickTime;
		splatDirection = direction;
	}
	
    public override PState Update(){
		wallSplatStickTimer -= Time.deltaTime;
		if(wallSplatStickTimer <= 0.0f){
			return new PStateWallSplatStumbling(splatDirection);
		}
		return this;
	}
	
	public override PState FixedUpdate(){
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
		return this;
	}
	
	public override PState LeaveGround(){
		return this;
	}
	
	public override PState LeaveWall(){
		return this;
	}
	
	public override PState ToggleJournal(){
		return this;
	}
}
