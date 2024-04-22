using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateFaceplantStanding : PState
{
	float standupTimer = 0.0f;

    public PStateFaceplantStanding(Vector2 exitVelocity){
		PState.rigidbody.velocity = new Vector2(0,0);
		standupTimer = PState.attr.cornerStunGetupTime;
		PState.player.animator.Play("PlayerStunFaceGetup");
	}
	
    public override PState Update(){
		if(standupTimer > 0){
			standupTimer -= Time.deltaTime;
			if(standupTimer <= 0){
				return new PStateIdle();
			}
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
