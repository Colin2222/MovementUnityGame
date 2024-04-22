using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateCornerClimbing : PState
{
    int cornerDir;
	float cornerClimbTimer;
	Transform corner;
	CornerHandler cornerHandler;

    public PStateCornerClimbing(){
		cornerHandler = PState.player.cornerHandler;
		corner = cornerHandler.corner;
		PState.rigidbody.gravityScale = 0f;
		PState.rigidbody.velocity = new Vector2(0f,0f);
		if(PState.player.transform.position.x > cornerHandler.corner.transform.position.x){
			cornerDir = 1;
		} else{
			cornerDir = -1;
		}
		
		PState.player.transform.position = new Vector3(cornerHandler.corner.position.x + (cornerHandler.cornerClimbOffsetX * cornerDir), cornerHandler.corner.position.y - cornerHandler.cornerClimbOffsetY, 0);
		cornerClimbTimer = PState.attr.cornerClimbTime;
		PState.player.animator.Play("PlayerCornerClimbing");
	}
	
    public override PState Update(){
		cornerClimbTimer -= Time.deltaTime;
		if(cornerClimbTimer <= 0){
			PState.player.transform.position = new Vector3(corner.position.x + (cornerHandler.cornerEndClimbOffsetX * cornerDir * -1), corner.position.y + cornerHandler.cornerEndClimbOffsetY, 0);
			PState.rigidbody.gravityScale = PState.attr.gravityScale;
			PState.direction = 0;
			PState.player.physics.isGrounded = false;
			return new PStateIdle();
		}
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
