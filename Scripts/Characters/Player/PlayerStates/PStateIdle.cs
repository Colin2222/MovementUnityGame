using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateIdle : PState
{
	public PStateIdle(PlayerHub player, PlayerInputManager inputManager, PlayerAttributeSet attr, Rigidbody2D rigidbody){
		PState.player = player;
		PState.inputManager = inputManager;
		PState.attr = attr;
		PState.rigidbody = rigidbody;
		PState.direction = 1;
	}
	
	public PStateIdle(){
		PState.player.animator.Play("PlayerIdle");
	}
	
    public override PState Update(){
		PState.timeSinceLastGroundHit += Time.deltaTime;
		return this;
	}
	
	public override PState FixedUpdate(){
		// apply resistive force to prevent some residual sliding after ending a slide stop/turn
		PState.rigidbody.AddForce(PState.rigidbody.velocity * PState.attr.moveForce * -1.0f, ForceMode2D.Force);
		return this;
	}
	
    public override PState HitGround(float hitSpeed){
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
		PState.player.animator.Play("PlayerJumpBracing");
		return new PStateJumpBracing();
	}
	
	public override PState ReleaseJump(){
		return this;
	}
	
	public override PState HitWall(Vector2 wallCollisionVelocity){
		return this;
	}
	
	public override PState Brace(){
		if(PState.player.cornerHandler.mantleCorner != null){
			return new PStateCornerMantling();
		} else if(PState.player.cornerHandler.corner != null){
			return new PStateCornerGrabbing();
		} else if(PState.timeSinceLastGroundHit < PState.attr.optionalRollWindow){
			return new PStateRolling();
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
}
