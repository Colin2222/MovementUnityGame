using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateFaceplantSoaring : PState
{
    public PStateFaceplantSoaring(Vector2 exitVelocity){
		rigidbody.velocity = exitVelocity * attr.cornerTripSpeedLoss;
		
		if(physics.isGrounded){
			this.HitGround(0.0f, 0.0f);
		}
	}
	
	public PStateFaceplantSoaring(){
		
	}

    public override PState Update(){
		return this;
	}

	public override PState FixedUpdate(){
		return this;
	}

    public override PState HitGround(float hitSpeedX, float hitSpeedY){
		if(hitSpeedY > attr.cornerStunReboundMinSpeed){
			player.animator.Play("PlayerFaceplantLanding");
		} else{
			if(hitSpeedY > 0.0f){
				player.animator.Play("PlayerFaceplantLaying");
			}
		}
		return new PStateFaceplantLaying();
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

	public override PState Grab(){
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
