using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateFaceplantLaying : PState
{
	float layTimer;
	bool laying; 
	
    public PStateFaceplantLaying(){
		laying = false; 
	}

    public override PState Update(){
		if(laying){
			layTimer -= Time.deltaTime;
			if(layTimer <= 0.0f){
				return new PStateFaceplantGettingUp();
			}
		}
		return this;
	}

	public override PState FixedUpdate(){
		PState.rigidbody.AddForce(PState.rigidbody.velocity * PState.attr.cornerStunSlideCoefficient * -1.0f, ForceMode2D.Force);
		if(!laying && Mathf.Abs(PState.rigidbody.velocity.x) < PState.attr.cornerStunGetupStartSpeed){
			laying = true;
			layTimer = PState.attr.cornerStunGetupWaitTime;
		}
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
		PState.player.animator.Play("PlayerFaceplantSoaring");
		return new PStateFaceplantSoaring();
	}

	public override PState LeaveWall(){
		return this;
	}

	public override PState ToggleJournal(){
		return this;
	}
}

