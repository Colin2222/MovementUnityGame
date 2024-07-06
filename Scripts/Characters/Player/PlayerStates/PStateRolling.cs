using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateRolling : PState
{
	float rollTimer;
	float rollTime;
	float minRollSpeed;
	bool runningJumpQueued;
	float jumpQueueTime;
	float rollCancelTime;
	bool slowRoll;
	Rigidbody2D rb;
	
    public PStateRolling(float hitSpeedX, float hitSpeedY){
		PState.player.animator.Play("PlayerRolling");
		minRollSpeed = PState.attr.groundRollMinSpeed;
		rb = PState.rigidbody;
		rollTime = PState.attr.groundRollTime;
		rollTimer = 0.0f;
		runningJumpQueued = false;
		jumpQueueTime = PState.attr.groundRollJumpQueueTime;
		rollCancelTime = PState.attr.groundRollEdgeCancelTime;
		
		// calculate if roll will be slow or full-speed
		float rollSpeedCalc = Mathf.Abs(hitSpeedX * 0.75f) + Mathf.Abs(hitSpeedY * 0.25f);
		if(rollSpeedCalc < PState.attr.groundRollSlowThreshold){
			minRollSpeed *= PState.attr.groundRollSlowMultiplier;
			slowRoll = true;
		} else{
			slowRoll = false;
		}
	}
	
    public override PState Update(){
		rollTimer += Time.deltaTime;
		if(rollTimer >= rollTime){
			CancelBraceCooldown();
			if(runningJumpQueued){
				if(slowRoll){
					PState.player.animator.Play("PlayerJumpBracing");
					return new PStateJumpBracing();
				}
				
				float jumpDir;
				if(PState.rigidbody.velocity.x > 0){
					jumpDir = 1.0f;
				} else{
					jumpDir = -1.0f;
				}
				PState.rigidbody.velocity = new Vector2(PState.attr.runningJumpSpeed * jumpDir, 0);
				PState.rigidbody.AddForce(new Vector2(0,PState.attr.jumpForce), ForceMode2D.Impulse);
				PState.player.animator.Play("PlayerJumpingRunning");
				return new PStateSoaring();
			} else{
				return new PStateMoving();
			}
		}
		return this;
	}
	
	public override PState FixedUpdate(){
		if(Mathf.Abs(rb.velocity.x) < minRollSpeed){
			float newVelo = minRollSpeed;
			if(rb.velocity.x == 0.0f){
				newVelo = newVelo * PState.direction;
			} else if(rb.velocity.x < 0.0f){
				newVelo = newVelo * -1;
			}
			
			rb.velocity = new Vector2(newVelo, rb.velocity.y);
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
		if(rollTimer > jumpQueueTime){
			runningJumpQueued = true;
		}
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
		if(runningJumpQueued && rollTimer > rollCancelTime){
			float jumpDir;
			if(PState.rigidbody.velocity.x > 0){
				jumpDir = 1.0f;
			} else{
				jumpDir = -1.0f;
			}
			PState.rigidbody.velocity = new Vector2(PState.attr.runningJumpSpeed * jumpDir, 0);
			PState.rigidbody.AddForce(new Vector2(0,PState.attr.jumpForce), ForceMode2D.Impulse);
			PState.player.animator.Play("PlayerJumpingRunning");
			return new PStateSoaring();
		}
		PState.player.animator.Play("PlayerSoaringStill");
		return new PStateSoaring();
	}
	
	public override PState LeaveWall(){
		return this;
	}
	
	public override PState ToggleJournal(){
		return this;
	}
}
