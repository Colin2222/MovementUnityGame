using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateRolling : PState
{
	float rollTimer;
	float rollTime;
	float minRollSpeed;
	Rigidbody2D rb;
	
    public PStateRolling(){
		PState.player.animator.Play("PlayerRolling");
		minRollSpeed = PState.attr.groundRollMinSpeed;
		rb = PState.rigidbody;
		rollTime = PState.attr.groundRollTime;
		rollTimer = 0.0f;
	}
	
    public override PState Update(){
		rollTimer += Time.deltaTime;
		if(rollTimer >= rollTime){
			return new PStateMoving();
		}
		return this;
	}
	
	public override PState FixedUpdate(){
		if(Mathf.Abs(rb.velocity.x) < minRollSpeed){
			float newVelo = minRollSpeed;
			if(PState.direction == -1){
				newVelo = newVelo * -1;
			}
			
			rb.velocity = new Vector2(newVelo, rb.velocity.y);
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
