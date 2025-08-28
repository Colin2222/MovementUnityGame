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
		wallSlideUpwardsCoefficient = attr.wallSlideUpwardsCoefficient;
		wallSlideDownwardsCoefficient = attr.wallSlideDownwardsCoefficient;
		this.wallCollisionVelocity = wallCollisionVelocity;
		wallBraceTimer = attr.wallBraceTime;
		player.animator.Play("PlayerWallBracing");
	}
	
    public override PState Update(){
		wallBraceTimer -= Time.deltaTime;
		if(wallBraceTimer <= 0){
			return new PStateWallPushing(wallCollisionVelocity);
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
		rigidbody.velocity = new Vector2((Mathf.Clamp(Mathf.Abs(wallCollisionVelocity.x) * attr.wallLaunchHorizontalRetention, attr.wallLaunchMinimumHorizontal, 10000.0f)) * jumpDir, Mathf.Clamp((wallCollisionVelocity.y * -1.0f) + attr.wallLaunchBoost, -1000f, attr.wallLaunchMaxVerticalSpeed));
		if(jumpDir == -1){
			player.transform.eulerAngles = new Vector2(0,180);
		} else{
			player.transform.eulerAngles = new Vector2(0,0);
		}

		// adjust position so player doesnt get caught on corners after flipping directions
		player.transform.position = new Vector3(player.transform.position.x + (jumpDir == -1 ? -0.05f : 0.05f), player.transform.position.y, 0.0f);

		return new PStateSoaring(true);
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
		if(player.cornerHandler.mantleCorner != null){
			if (inputManager.bracing && Mathf.Abs(wallCollisionVelocity.x) > attr.fastMantleMinimumHorizontalSpeed && rigidbody.velocity.y > attr.fastMantleMinimumVerticalSpeed)
			{
				return new PStateCornerMantlingFast(new Vector2(wallCollisionVelocity.x, rigidbody.velocity.y));
			}
			return new PStateCornerMantling();
		} else if(player.cornerHandler.corner != null){
			return new PStateCornerGrabbing(Mathf.Abs(lastAirSpeed) - Mathf.Clamp(rigidbody.velocity.y, -100f, 0.0f));
		}
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
