using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateIdle : PState
{
	public PStateIdle(PlayerHub player, PlayerInputManager inputManager, PlayerAttributeSet attr, Rigidbody2D rigidbody, CharacterPhysicsChecker physics){
		PState.player = player;
		PState.inputManager = inputManager;
		PState.attr = attr;
		PState.rigidbody = rigidbody;
		PState.physics = physics;
		PState.direction = 1;
	}
	
	public PStateIdle(){
		player.animator.Play("PlayerIdle");
	}
	
    public override PState Update(){
		timeSinceLastGroundHit += Time.deltaTime;
		return this;
	}
	
	public override PState FixedUpdate(){
		// apply resistive force to prevent some residual sliding after ending a slide stop/turn
		rigidbody.AddForce(rigidbody.velocity * attr.moveForce * -1.0f, ForceMode2D.Force);
		return this;
	}
	
    public override PState HitGround(float hitSpeedX, float hitSpeedY){
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
		player.animator.Play("PlayerJumpBracing");
		return new PStateJumpBracing();
	}
	
	public override PState ReleaseJump(){
		return this;
	}
	
	public override PState HitWall(Vector2 wallCollisionVelocity, WallCollisionInfo collInfo){
		return this;
	}
	
	public override PState Brace(){
		if(player.cornerHandler.CheckFootHandler(direction)){
			return new PStateCornerClimbingDown(direction);
		} else if(player.cornerHandler.mantleCorner != null){
			return new PStateCornerMantling();
		} else if(player.cornerHandler.corner != null){
			return new PStateCornerGrabbing();
		} else if(timeSinceLastGroundHit < attr.optionalRollWindow){
			return new PStateRolling(physics.lastBottomCollisionSpeed.x, physics.lastBottomCollisionSpeed.y);
		}
		return this;
	}
	
	public override PState LeaveGround(){
		player.animator.Play("PlayerSoaringStill");
		return new PStateSoaring();
	}
	
	public override PState LeaveWall(){
		return this;
	}
	
	public override PState ToggleJournal(){
		return new PStateViewingJournal();
	}
}
