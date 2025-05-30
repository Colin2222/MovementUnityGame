using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateWallPushing : PState
{
	float wallPushTimer;
	Vector2 wallCollisionVelocity;
	float wallPushLaunchCoefficient;
	float wallSlideUpwardsCoefficient;
	float wallSlideDownwardsCoefficient;
	
    public PStateWallPushing(Vector2 wallCollisionVelocity){
		wallPushLaunchCoefficient = attr.wallPushLaunchCoefficient;
		wallSlideUpwardsCoefficient = attr.wallSlideUpwardsCoefficient;
		wallSlideDownwardsCoefficient = attr.wallSlideDownwardsCoefficient;
		this.wallCollisionVelocity = wallCollisionVelocity;
		wallPushTimer = attr.wallPushTime;
		player.animator.Play("PlayerWallPushing");
	}
	
    public override PState Update(){
		wallPushTimer -= Time.deltaTime;
		if(wallPushTimer <= 0){
			rigidbody.velocity = new Vector2(wallCollisionVelocity.x * attr.wallPushHorizontalRetention, rigidbody.velocity.y + attr.wallPushBoost);
			return new PStateSoaring();
		}
		return this;
	}
	
	public override PState FixedUpdate(){
		if(rigidbody.velocity.y > 0){
			rigidbody.AddForce(rigidbody.velocity * -1.0f * wallSlideUpwardsCoefficient, ForceMode2D.Force);
		} else{
			rigidbody.AddForce(rigidbody.velocity * -1.0f * wallSlideDownwardsCoefficient, ForceMode2D.Force);
		}
		return this;
	}
	
    public override PState HitGround(float hitSpeedX, float hitSpeedY){
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
		player.animator.Play("PlayerWallLaunching");
		player.soundInterface.PlayWallJump();
		float jumpDir = Mathf.Sign(wallCollisionVelocity.x);
		rigidbody.velocity = new Vector2((Mathf.Clamp(Mathf.Abs(wallCollisionVelocity.x) * attr.wallPushToLaunchHorizontalRetention, attr.wallLaunchMinimumHorizontal, 10000.0f)) * jumpDir, Mathf.Clamp((wallCollisionVelocity.y * -1.0f) + attr.wallPushToLaunchBoost, -1000f, attr.wallLaunchMaxVerticalSpeed));
		if(jumpDir == -1){
			player.transform.eulerAngles = new Vector2(0,180);
		} else{
			player.transform.eulerAngles = new Vector2(0,0);
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

	public override PState Grab(){
		return this;
	}
	
	public override PState LeaveGround(){
		return this;
	}
	
	public override PState LeaveWall(){
		player.animator.Play("PlayerSoaringStill");
		return new PStateSoaring();
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
