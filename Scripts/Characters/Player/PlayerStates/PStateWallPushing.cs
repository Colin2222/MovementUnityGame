using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateWallPushing : PState
{
	float wallPushTimer;
	Vector2 wallCollisionVelocity;
	float wallPushLaunchCoefficient;
	
    public PStateWallPushing(Vector2 wallCollisionVelocity){
		wallPushLaunchCoefficient = PState.attr.wallPushLaunchCoefficient;
		this.wallCollisionVelocity = wallCollisionVelocity;
		wallPushTimer = PState.attr.wallPushTime;
		PState.player.animator.Play("PlayerWallPushing");
	}
	
    public override PState Update(){
		wallPushTimer -= Time.deltaTime;
		if(wallPushTimer <= 0){
			PState.rigidbody.velocity = new Vector2(wallCollisionVelocity.x * PState.attr.wallPushHorizontalRetention, PState.rigidbody.velocity.y + PState.attr.wallPushBoost);
			return new PStateSoaring();
		}
		return this;
	}
	
	public override PState FixedUpdate(){
		PState.rigidbody.AddForce(PState.rigidbody.velocity * -1.0f * wallPushLaunchCoefficient, ForceMode2D.Force);
		return this;
	}
	
    public override PState HitGround(float hitSpeed){
		return new PStateMoving();
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
		PState.player.animator.Play("PlayerWallLaunching");
		float jumpDir = Mathf.Sign(wallCollisionVelocity.x);
		PState.rigidbody.velocity = new Vector2((Mathf.Clamp(Mathf.Abs(wallCollisionVelocity.x) * PState.attr.wallPushToLaunchHorizontalRetention, PState.attr.wallLaunchMinimumHorizontal, 10000.0f)) * jumpDir, Mathf.Clamp((wallCollisionVelocity.y * -1.0f) + PState.attr.wallPushToLaunchBoost, -1000f, PState.attr.wallLaunchMaxVerticalSpeed));
		if(jumpDir == -1){
			PState.player.transform.eulerAngles = new Vector2(0,180);
		} else{
			PState.player.transform.eulerAngles = new Vector2(0,0);
		}
		return new PStateSoaring();
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
		return new PStateSoaring();
	}
	
	public override PState ToggleJournal(){
		return this;
	}
}
