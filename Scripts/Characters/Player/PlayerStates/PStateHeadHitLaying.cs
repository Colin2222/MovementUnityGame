using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateHeadHitLaying : PState
{
    float layTimer;
	bool laying; 
	
    public PStateHeadHitLaying(){
		laying = false; 
	}

    public override PState Update(){
		if(laying){
			layTimer -= Time.deltaTime;
			if(layTimer <= 0.0f){
				return new PStateHeadHitGettingUp();
			}
		}
		return this;
	}

	public override PState FixedUpdate(){
		rigidbody.AddForce(rigidbody.velocity * attr.cornerStunSlideCoefficient * -1.0f, ForceMode2D.Force);
		if(!laying && Mathf.Abs(rigidbody.velocity.x) < attr.cornerStunGetupStartSpeed){
			laying = true;
			layTimer = attr.cornerStunGetupWaitTime;
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

	public override PState Grab(){
		return this;
	}

	public override PState LeaveGround(){
		return new PStateFaceplantSoaring();
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
