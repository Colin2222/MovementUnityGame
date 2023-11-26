using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateWallBracing : PState
{
	Vector2 wallCollisionVelocity;
	float wallBraceTimer;
	float wallSlideUpwardsCoefficient;
	float wallSlideDownwardsCoefficient;
	
    public PStateWallBracing(Vector2 wallCollisionVelocity){
		wallSlideUpwardsCoefficient = PState.attr.wallSlideUpwardsCoefficient;
		wallSlideDownwardsCoefficient = PState.attr.wallSlideDownwardsCoefficient;
		this.wallCollisionVelocity = wallCollisionVelocity;
		wallBraceTimer = PState.attr.wallBraceTime;
		PState.player.animator.Play("PlayerWallBracing");
	}
	
    public override PState Update(){
		wallBraceTimer -= Time.deltaTime;
		if(wallBraceTimer <= 0){
			return new PStateWallPushing(wallCollisionVelocity);
		}
		return this;
	}
	
	public override PState FixedUpdate(){
		if(PState.rigidbody.velocity.y > 0){
			PState.rigidbody.AddForce(PState.rigidbody.velocity * -1.0f * wallSlideUpwardsCoefficient, ForceMode2D.Force);
		} else{
			PState.rigidbody.AddForce(PState.rigidbody.velocity * -1.0f * wallSlideDownwardsCoefficient, ForceMode2D.Force);
		}
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
		PState.rigidbody.velocity = new Vector2((Mathf.Clamp(Mathf.Abs(wallCollisionVelocity.x) * PState.attr.wallLaunchHorizontalRetention, PState.attr.wallLaunchMinimumHorizontal, 10000.0f)) * jumpDir, Mathf.Clamp((wallCollisionVelocity.y * -1.0f) + PState.attr.wallLaunchBoost, -1000f, PState.attr.wallLaunchMaxVerticalSpeed));
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
	
	public override PState HitWall(Vector2 wallCollisionVelocity){
		return this;
	}
	
	public override PState Brace(){
		if(PState.player.cornerHandler.mantleCorner != null){
			return new PStateCornerMantling();
		} else if(PState.player.cornerHandler.corner != null){
			return new PStateCornerGrabbing();
		}
		return this;
	}
	
	public override PState LeaveGround(){
		return this;
	}
	
	public override PState LeaveWall(){
		return new PStateSoaring();
	}
}
