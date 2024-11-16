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
		cornerHandler = player.cornerHandler;
		mantleCorner = cornerHandler.mantleCorner;
		rigidbody.gravityScale = 0f;
		rigidbody.velocity = new Vector2(0f,0f);
		if(player.transform.position.x > cornerHandler.mantleCorner.transform.position.x){
			cornerDir = 1;
		} else{
			cornerDir = -1;
		}
		
		player.transform.position = new Vector3(cornerHandler.mantleCorner.position.x + (cornerHandler.mantleClimbOffsetX * cornerDir), cornerHandler.mantleCorner.position.y - cornerHandler.mantleClimbOffsetY, 0);
		cornerClimbTimer = attr.cornerMantleTime;
		player.animator.Play("PlayerCornerMantling");
	}
	
    public override PState Update(){
		cornerClimbTimer -= Time.deltaTime;
		if(cornerClimbTimer <= 0){
			player.transform.position = new Vector3(mantleCorner.position.x + (cornerHandler.cornerEndClimbOffsetX * cornerDir * -1), mantleCorner.position.y + cornerHandler.cornerEndClimbOffsetY, 0);
			rigidbody.gravityScale = attr.gravityScale;
			player.physics.isGrounded = false;
			SetDirection(-cornerDir);
			return new PStateIdle();
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
