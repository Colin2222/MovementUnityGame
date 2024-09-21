using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateHeadHitGettingUp : PState
{
    float getupTimer;
	
    public PStateHeadHitGettingUp(){
		// prevents clipping when switching hitboxes
		PState.player.transform.position += new Vector3(0.0f, 0.5f, 0.0f);
		
		PState.physics.SwitchHitboxes(1);
		PState.player.animator.Play("PlayerHeadHitGettingUp");
		getupTimer = PState.attr.cornerBackStunGetupTime;
	}

    public override PState Update(){
		getupTimer -= Time.deltaTime;
		if(getupTimer <= 0.0f){
			PState.player.animator.Play("PlayerIdle");
			return new PStateIdle();
		}
		return this;
	}

	public override PState FixedUpdate(){
		PState.rigidbody.AddForce(PState.rigidbody.velocity * PState.attr.cornerStunSlideCoefficient * -1.0f, ForceMode2D.Force);
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
