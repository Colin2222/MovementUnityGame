using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateCornerClimbingDown : PState
{
    int cornerDir;
	float cornerClimbTimer;
	Transform footCorner;
	CornerHandler cornerHandler;

    public PStateCornerClimbingDown(int direction){
		cornerHandler = player.cornerHandler;
		footCorner = cornerHandler.footCorner;
		rigidbody.gravityScale = 0f;
		rigidbody.velocity = new Vector2(0f,0f);
		cornerDir = direction;
		SetDirection(-cornerDir);
		
		player.transform.position = new Vector3(cornerHandler.footCorner.position.x + (cornerHandler.cornerClimbOffsetX * cornerDir), cornerHandler.footCorner.position.y - cornerHandler.cornerClimbOffsetY, 0);
		cornerClimbTimer = attr.cornerClimbTime;
		player.animator.Play("PlayerCornerClimbingDown");
	}
	
    public override PState Update(){
		cornerClimbTimer -= Time.deltaTime;
		if(cornerClimbTimer <= 0){
			return new PStateCornerGrabbing();
		}
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
