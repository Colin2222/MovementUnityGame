using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateSlideStopping : PState
{
    public PStateSlideStopping(){
		
	}
	
    public override PState Update(){
		player.animator.Play("PlayerSlideStopping");
		timeSinceLastGroundHit += Time.deltaTime;
		return this;
	}
	
	public override PState FixedUpdate(){
		// apply resistive force
		rigidbody.AddForce(rigidbody.velocity * attr.moveForce * -1.0f * attr.slideForceMultiplier, ForceMode2D.Force);
		
		// check if player has slowed down enough to exit slide
		if(Mathf.Abs(rigidbody.velocity.x) < attr.slideStopSpeedTarget){
			return new PStateIdle();
		}
		return this;
	}
	
    public override PState HitGround(float hitSpeedX, float hitSpeedY){
		return this;
	}
	
	public override PState Move(float horizontal, float vertical){
		if(Mathf.Sign(horizontal) != Mathf.Sign(rigidbody.velocity.x) && horizontal != 0.0f){
			return new PStateSlideTurning();
		} else if(Mathf.Sign(horizontal) == Mathf.Sign(rigidbody.velocity.x) && horizontal != 0.0f){
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
		if(Mathf.Abs(rigidbody.velocity.x) > attr.runningJumpSpeed){
			rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
			rigidbody.AddForce(new Vector2(0,attr.jumpForce), ForceMode2D.Impulse);
			player.animator.Play("PlayerJumpingRunning");
			return new PStateSoaring();
		} else{
			player.animator.Play("PlayerJumpBracing");
			return new PStateJumpBracing();
		}
	}
	
	public override PState ReleaseJump(){
		return this;
	}
	
	public override PState HitWall(Vector2 wallCollisionVelocity, WallCollisionInfo collInfo){
		if(Mathf.Abs(wallCollisionVelocity.x) > attr.wallSplatMinSpeed){
			return new PStateWallSplatting((int)Mathf.Sign(wallCollisionVelocity.x));
		}
		
		return this;
	}
	
	public override PState Brace(){
		if(timeSinceLastGroundHit < attr.optionalRollWindow){
			return new PStateRolling(physics.lastBottomCollisionSpeed.x, physics.lastBottomCollisionSpeed.y);
		}
		return this;
	}

	public override PState Grab(){
		if(Mathf.Abs(rigidbody.velocity.x) < attr.cornerClimbDownMaxEntrySpeed && player.cornerHandler.CheckFootHandler(direction)){
			return new PStateCornerClimbingDown(direction);
		} else if(player.cornerHandler.mantleCorner != null){
			return new PStateCornerMantling();
		} else if(player.cornerHandler.corner != null){
			return new PStateCornerGrabbing(0.0f);
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

	public override PState Interact(){
		return this;
	}
	
	public override PState ToggleJournal(){
		return this;
	}

	public override PState ToggleInventory(){
		return this;
	}
}
