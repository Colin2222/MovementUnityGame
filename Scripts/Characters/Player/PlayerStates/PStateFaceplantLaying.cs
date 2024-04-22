using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateFaceplantLaying : PState
{
	float standupTimer = 0.0f;

    public PStateFaceplantLaying(Vector2 exitVelocity){
		PState.rigidbody.velocity = exitVelocity * PState.attr.cornerTripSpeedLoss;
		PState.player.animator.Play("PlayerStunFace");
	}
	
    public override PState Update(){
		if(standupTimer > 0){
			standupTimer -= Time.deltaTime;
			if(standupTimer <= 0){
				
			}
		}
		return this;
	}
	
	public override PState FixedUpdate(){
		// apply resistive force
		PState.rigidbody.AddForce(PState.rigidbody.velocity * PState.attr.cornerStunSpeedCoefficient * -1.0f, ForceMode2D.Force);
		
		// check if slow enough to start timer to get up
		if(Mathf.Abs(PState.rigidbody.velocity.x) <= PState.attr.cornerStunGetupStartSpeed){
			standupTimer = PState.attr.cornerStunGetupWaitTime;
		}
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
