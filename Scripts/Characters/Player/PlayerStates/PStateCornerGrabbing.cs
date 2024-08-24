using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateCornerGrabbing : PState
{	
	int cornerDir;
	float horizontal;
	float vertical;

    public PStateCornerGrabbing(){
		CornerHandler cornerHandler = PState.player.cornerHandler;
		PState.rigidbody.gravityScale = 0f;
		PState.rigidbody.velocity = new Vector2(0f,0f);
		if(PState.player.transform.position.x > cornerHandler.corner.transform.position.x){
			cornerDir = 1;
		} else{
			cornerDir = -1;
		}
		
		PState.player.transform.position = new Vector3(cornerHandler.corner.position.x + (cornerHandler.cornerOffsetX * cornerDir), cornerHandler.corner.position.y - cornerHandler.cornerOffsetY, 0);
		PState.player.soundInterface.PlayCornerGrab();
		PState.player.animator.Play("PlayerCornerGrabbing");
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
		PState.rigidbody.gravityScale = PState.attr.gravityScale;
		PState.player.animator.Play("PlayerSoaringStill");
		return new PStateSoaring();
	}
	
	public override PState PressJump(){
		if(Mathf.Abs(horizontal) >= PState.attr.cornerJumpHorizontalJoystickThreshold && Mathf.Sign(cornerDir) == Mathf.Sign(horizontal) && PState.physics.frontCheck.IsLowerContact(cornerDir)){
			float aimAngle = Mathf.Atan2(vertical, horizontal);
			float horizontalForce = PState.attr.cornerJumpForce * Mathf.Cos(aimAngle);
			float verticalForce = PState.attr.cornerJumpForce * Mathf.Sin(aimAngle);
			
			horizontalForce *= PState.attr.cornerJumpHorizontalForceCoefficient;
			if(Mathf.Abs(horizontalForce) > PState.attr.cornerJumpMaximumHorizontalForce){
				horizontalForce = PState.attr.cornerJumpMaximumHorizontalForce * cornerDir;
			}

			if(verticalForce > 0.0f && verticalForce >= PState.attr.cornerJumpMaximumVerticalForce){
				verticalForce *= PState.attr.cornerJumpVerticalForceCoefficient;
			} else if(verticalForce < PState.attr.cornerJumpMaximumVerticalForce){
				verticalForce = PState.attr.cornerJumpMaximumVerticalForce;
			}
			rigidbody.AddForce((new Vector2(horizontalForce, verticalForce)) * (new Vector2(horizontal, vertical)).magnitude, ForceMode2D.Impulse);
			
			SetDirection(horizontal);
			PState.rigidbody.gravityScale = PState.attr.gravityScale;
			PState.player.animator.Play("PlayerWallPushing");
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
		PState.rigidbody.gravityScale = PState.attr.gravityScale;
		PState.player.animator.Play("PlayerSoaringStill");
		return new PStateSoaring();
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
