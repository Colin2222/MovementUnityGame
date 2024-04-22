using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateCornerTripping : PState
{
    int cornerDir;
	float tripTimer;
	Transform corner;
	CornerHandler cornerHandler;
	Vector2 exitVelocity;

    public PStateCornerTripping(Vector2 wallCollisionVelocity){
		exitVelocity = wallCollisionVelocity * -1.0f;
		cornerHandler = PState.player.cornerHandler;
		corner = cornerHandler.trackedCorner;
		if(PState.player.transform.position.x > cornerHandler.trackedCorner.transform.position.x){
			cornerDir = 1;
		} else{
			cornerDir = -1;
		}
		
		PState.player.transform.position = new Vector3(corner.position.x + (cornerHandler.cornerEndClimbOffsetX * cornerDir * -1), corner.position.y + cornerHandler.cornerEndClimbOffsetY, 0);
		tripTimer = PState.attr.cornerTripTime;
		PState.player.animator.Play("TESTINGANIM");
	}
	
    public override PState Update(){
		return new PStateFaceplantSoaring(exitVelocity);
		/*
		tripTimer -= Time.deltaTime;
		if(tripTimer <= 0){
			PState.player.transform.position = new Vector3(corner.position.x + (cornerHandler.cornerEndClimbOffsetX * cornerDir * -1), corner.position.y + cornerHandler.cornerEndClimbOffsetY, 0);
			PState.rigidbody.gravityScale = PState.attr.gravityScale;
			PState.direction = 0;
			PState.player.physics.isGrounded = false;
			Debug.Log("SOARING");
			return new PStateFaceplantSoaring(exitVelocity);
		}
		*/
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

