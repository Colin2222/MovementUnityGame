using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateCornerHeadHitting : PState
{
    int cornerDir;
	float tripTimer;
	Transform corner;
	CornerHandler cornerHandler;
	Vector2 exitVelocity;

    public PStateCornerHeadHitting(Vector2 wallCollisionVelocity){
		exitVelocity = wallCollisionVelocity * -1.0f;

		player.transform.position += new Vector3(0.0f, -0.5f, 0.0f);
		//player.transform.position = new Vector3(corner.position.x + (cornerHandler.cornerEndClimbOffsetX * cornerDir * -1), corner.position.y + cornerHandler.cornerEndClimbOffsetY, 0);
		tripTimer = attr.cornerTripTime;
		player.animator.Play("PlayerCornerHeadHitting");
	}

    public override PState Update(){
		physics.SwitchHitboxes(2);
		return new PStateHeadHitSoaring(exitVelocity);
		/*
		tripTimer -= Time.deltaTime;
		if(tripTimer <= 0){
			player.transform.position = new Vector3(corner.position.x + (cornerHandler.cornerEndClimbOffsetX * cornerDir * -1), corner.position.y + cornerHandler.cornerEndClimbOffsetY, 0);
			rigidbody.gravityScale = attr.gravityScale;
			direction = 0;
			player.physics.isGrounded = false;
			Debug.Log("SOARING");
			return new PStateFaceplantSoaring(exitVelocity);
		}
		*/
		return this;
	}

	public override PState FixedUpdate(){
		return this;
	}

    public override PState HitGround(float hitSpeedX, float hitSpeedY){
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
