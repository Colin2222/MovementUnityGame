using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateMoving : PState
{
	float horizontal;
	float vertical;
	bool isFast = false;
	bool isSlow = false;

	public PStateMoving()
	{
		player.soundInterface.PlayStep2();
		player.animator.Play("PlayerRunning");
		isFast = false;
		isSlow = false;
		HandleFastMovement();
	}
	
    public override PState Update(){
		HandleFastMovement();
		timeSinceLastGroundHit += Time.deltaTime;
		return this;
	}

	void HandleFastMovement()
	{
		if (!isFast && Mathf.Abs(rigidbody.velocity.x) > attr.runningJumpSpeed)
		{
			isFast = true;
		}
		else if (isFast && Mathf.Abs(rigidbody.velocity.x) < attr.runningJumpSpeed)
		{
			isFast = false;
		}
		else if (!isSlow && Mathf.Abs(rigidbody.velocity.x) < attr.slowRunSpeed)
		{
			isSlow = true;
		}
		else if (isSlow && Mathf.Abs(rigidbody.velocity.x) > attr.slowRunSpeed)
		{
			isSlow = false;
		}

		if (isFast || isSlow)
		{
			player.animator.Play("PlayerRunning");
		}
		else
		{
			player.animator.Play("PlayerRunningFast");
		}
	}
	
	public override PState FixedUpdate(){
		// apply running force
		rigidbody.AddForce(new Vector2(horizontal, 0.0f) * attr.moveForce, ForceMode2D.Force);
		
		// hold player under maximum run speed
		if(Mathf.Abs(rigidbody.velocity.x) > attr.maxRunSpeed){
			rigidbody.velocity = new Vector2(attr.maxRunSpeed * Mathf.Sign(rigidbody.velocity.x), rigidbody.velocity.y);
		}
		
		return this;
	}
	
    public override PState HitGround(float hitSpeedX, float hitSpeedY){
		return this;
	}
	
	public override PState Move(float horizontal, float vertical){
		this.horizontal = horizontal;
		this.vertical = vertical;
		
		if(horizontal == 0.0f){
			base.SetDirection(horizontal);
			return new PStateSlideStopping();
		} else if(Mathf.Sign(horizontal) != Mathf.Sign(rigidbody.velocity.x) && Mathf.Abs(rigidbody.velocity.x) > attr.slideStopSpeedTarget){
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
		if(Mathf.Abs(rigidbody.velocity.x) > attr.runningJumpSpeed){
			rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
			rigidbody.AddForce(new Vector2(0,attr.jumpForce), ForceMode2D.Impulse);
			player.soundInterface.PlayStillJump();
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
		if(Mathf.Abs(rigidbody.velocity.x) > attr.runningJumpSpeed){
			return new PStateSoaring(attr.runningJumpCoyoteTime);
		}
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
