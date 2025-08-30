using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateSoaring : PState
{
	float coyoteTimer = 0.0f;
	Vector2 lastVelo;

	// set to true when coming out of a wall jump, as in "the player is in a chain of actions" 
	// used to round up running speed to avoid losing enough speed on chained wall jumps to trigger stationary jumps instead of running jumps
	bool inChain = false;

    public PStateSoaring()
	{

	}

	public PStateSoaring(float coyoteTime){
		coyoteTimer = coyoteTime;
	}

	public PStateSoaring(bool inChain)
	{
		this.inChain = inChain;
	}
	
    public override PState Update()
	{
		if (coyoteTimer > 0.0f)
		{
			coyoteTimer -= Time.deltaTime;
		}
		return this;
	}
	
	public override PState FixedUpdate(){
		lastAirSpeed = rigidbody.velocity.x;
		if (lastVelo == rigidbody.velocity && !physics.isGrounded && physics.frontCheck.feetCornerTouching)
		{
			Debug.Log("Player stuck FIXED");
			return new PStateCornerFaceplanting(new Vector2(direction, 0.0f));
		}
		lastVelo = rigidbody.velocity;
		return this;
	}
	
    public override PState HitGround(float hitSpeedX, float hitSpeedY){
		if(hitSpeedY > attr.groundHitSpeedRollThreshold){
			return new PStateRollEntering();
		} else if(hitSpeedY > attr.groundHitSpeedRollMin && inputManager.bracing){
			if(hitSpeedY > attr.groundHitSpeedRollThreshold){
				player.soundInterface.PlayStillJumpLand();
			}
			return new PStateRolling(hitSpeedX, hitSpeedY);
		}
		timeSinceLastGroundHit = 0.0f;
		lastGroundHitSpeed = hitSpeedY;

		// round up horizontal speed if in chain
		if (inChain)
		{
			if (Mathf.Abs(rigidbody.velocity.x) > attr.chainMinHorizontalSpeed && Mathf.Abs(rigidbody.velocity.x) < attr.chainMaxHorizontalSpeed)
			{
				rigidbody.velocity = new Vector2(attr.chainMaxHorizontalSpeed * direction, rigidbody.velocity.y);
			}
			inChain = false;
		}
		return new PStateMoving();
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
		if(coyoteTimer > 0.0f){
			rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
			rigidbody.AddForce(new Vector2(0,attr.jumpForce), ForceMode2D.Impulse);
			player.soundInterface.PlayStillJump();
			player.animator.Play("PlayerJumpingRunning");
			return new PStateSoaring();
		}
		return this;
	}
	
	public override PState ReleaseJump(){
		return this;
	}
	
	public override PState HitWall(Vector2 wallCollisionVelocity, WallCollisionInfo collInfo){
		if(!collInfo.touchFaceplant && collInfo.touchFeet && Mathf.Abs(wallCollisionVelocity.x) > attr.cornerTripMinimumSpeed){
			return new PStateCornerFaceplanting(wallCollisionVelocity);
		} else if(!collInfo.touchLowerHead && collInfo.touchHead && Mathf.Abs(wallCollisionVelocity.x) > attr.cornerTripMinimumSpeed){
			return new PStateCornerHeadHitting(wallCollisionVelocity);
		}
		
		if(Mathf.Abs(wallCollisionVelocity.x) > 0.0f){
			return new PStateWallBracing(wallCollisionVelocity);
		} else{
			return this;
		}
	}
	
	public override PState Brace(){
		return this;
	}

	public override PState Grab(){
		if(player.cornerHandler.mantleCorner != null){
			if (inputManager.isBracePressed() && Mathf.Abs(rigidbody.velocity.x) > attr.fastMantleMinimumHorizontalSpeed && rigidbody.velocity.y > attr.fastMantleMinimumVerticalSpeed)
			{
				return new PStateCornerMantlingFast(rigidbody.velocity * new Vector2(-1, 1));
			}
			return new PStateCornerMantling();
		} else if(player.cornerHandler.corner != null){
			return new PStateCornerGrabbing(Mathf.Abs(lastAirSpeed) - Mathf.Clamp(rigidbody.velocity.y, -100f, 0.0f));
		}
		return this;
	}
	
	public override PState LeaveGround(){
		return this;
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
