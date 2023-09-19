using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateCornerMantling : PState
{
    int cornerDir;
	float cornerClimbTimer;
	Transform mantleCorner;
	CornerHandler cornerHandler;

    public PStateCornerMantling(){
		cornerHandler = PState.player.cornerHandler;
		mantleCorner = cornerHandler.mantleCorner;
		PState.rigidbody.gravityScale = 0f;
		PState.rigidbody.velocity = new Vector2(0f,0f);
		if(PState.player.transform.position.x > cornerHandler.mantleCorner.transform.position.x){
			cornerDir = 1;
		} else{
			cornerDir = -1;
		}
		
		PState.player.transform.position = new Vector3(cornerHandler.mantleCorner.position.x + (cornerHandler.mantleClimbOffsetX * cornerDir), cornerHandler.mantleCorner.position.y - cornerHandler.mantleClimbOffsetY, 0);
		cornerClimbTimer = PState.attr.cornerMantleTime;
		PState.player.animator.Play("PlayerCornerMantling");
	}
	
    public override PState Update(){
		cornerClimbTimer -= Time.deltaTime;
		if(cornerClimbTimer <= 0){
			PState.player.transform.position = new Vector3(mantleCorner.position.x + (cornerHandler.cornerEndClimbOffsetX * cornerDir * -1), mantleCorner.position.y + cornerHandler.cornerEndClimbOffsetY, 0);
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
	
	public override PState HitWall(Vector2 wallCollisionVelocity){
		return this;
	}
	
	public override PState Brace(){
		return this;
	}
	
	public override PState LeaveGround(){
		return this;
	}
}
