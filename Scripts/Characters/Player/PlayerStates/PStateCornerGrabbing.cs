using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateCornerGrabbing : PState
{	
	int cornerDir;
	float horizontal;
	float vertical;

    public PStateCornerGrabbing(){
		CornerHandler cornerHandler = player.cornerHandler;
		rigidbody.gravityScale = 0f;
		rigidbody.velocity = new Vector2(0f,0f);
		if(player.transform.position.x > cornerHandler.corner.transform.position.x){
			cornerDir = 1;
		} else{
			cornerDir = -1;
		}
		
		player.transform.position = new Vector3(cornerHandler.corner.position.x + (cornerHandler.cornerOffsetX * cornerDir), cornerHandler.corner.position.y - cornerHandler.cornerOffsetY, 0);
		player.soundInterface.PlayCornerGrab();
		player.animator.Play("PlayerCornerGrabbing");
	}
	
    public override PState Update(){
		return this;
	}
	
	public override PState FixedUpdate(){
		return this;
	}
	
    public override PState HitGround(float hitSpeedX, float hitSpeedY){
		return this;
	}
	
	public override PState Move(float horizontal, float vertical){
		this.horizontal = horizontal;
		this.vertical = vertical;
		return this;
	}
	
	public override PState ClimbUp(){
		return new PStateCornerClimbing();
	}
	
	public override PState ClimbDown(){
		rigidbody.gravityScale = attr.gravityScale;
		player.animator.Play("PlayerSoaringStill");
		return new PStateSoaring();
	}
	
	public override PState PressJump(){
		if(Mathf.Abs(horizontal) >= attr.cornerJumpHorizontalJoystickThreshold && Mathf.Sign(cornerDir) == Mathf.Sign(horizontal) && physics.frontCheck.IsLowerContact(cornerDir)){
			float aimAngle = Mathf.Atan2(vertical, horizontal);
			float horizontalForce = attr.cornerJumpForce * Mathf.Cos(aimAngle);
			float verticalForce = attr.cornerJumpForce * Mathf.Sin(aimAngle);
			
			horizontalForce *= attr.cornerJumpHorizontalForceCoefficient;
			if(Mathf.Abs(horizontalForce) > attr.cornerJumpMaximumHorizontalForce){
				horizontalForce = attr.cornerJumpMaximumHorizontalForce * cornerDir;
			}

			if(verticalForce > 0.0f && verticalForce >= attr.cornerJumpMaximumVerticalForce){
				verticalForce *= attr.cornerJumpVerticalForceCoefficient;
			} else if(verticalForce < attr.cornerJumpMaximumVerticalForce){
				verticalForce = attr.cornerJumpMaximumVerticalForce;
			}
			rigidbody.AddForce((new Vector2(horizontalForce, verticalForce)) * (new Vector2(horizontal, vertical)).magnitude, ForceMode2D.Impulse);
			
			SetDirection(horizontal);
			rigidbody.gravityScale = attr.gravityScale;
			player.animator.Play("PlayerWallPushing");
			return new PStateSoaring();
		}
		return this;
	}
	
	public override PState ReleaseJump(){
		return this;
	}
	
	public override PState HitWall(Vector2 wallCollisionVelocity, WallCollisionInfo collInfo){
		return this;
	}
	
	public override PState Brace(){
		rigidbody.gravityScale = attr.gravityScale;
		if(physics.isGrounded){
			return new PStateIdle();
		}
		physics.ClearBottomCheck();
		player.animator.Play("PlayerSoaringStill");
		return new PStateSoaring();
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
