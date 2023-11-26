using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateCornerGrabbing : PState
{	
	int cornerDir;

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
	
    public override PState HitGround(float hitSpeed){
		return this;
	}
	
	public override PState Move(float horizontal, float vertical){
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
		return this;
	}
	
	public override PState ReleaseJump(){
		return this;
	}
	
	public override PState HitWall(Vector2 wallCollisionVelocity){
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
}
