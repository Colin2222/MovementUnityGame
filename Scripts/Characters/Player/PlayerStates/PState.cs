using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PState
{
	protected static PlayerHub player;
	protected static PlayerInputManager inputManager;
	protected static PlayerInteractor interactor;
	protected static PlayerAttributeSet attr;
	protected static Rigidbody2D rigidbody;
	protected static CharacterPhysicsChecker physics;
	protected static int direction;
	protected static float timeSinceLastGroundHit;
	protected static float lastGroundHitSpeed;
	protected static float lastAirSpeed;
	
	public abstract PState Update();
	public abstract PState FixedUpdate();
	public abstract PState Move(float horizontal, float vertical);
	public abstract PState HitGround(float hitX, float hitY);
	public abstract PState LeaveGround();
	public abstract PState LeaveWall();
	public abstract PState ClimbUp();
	public abstract PState ClimbDown();
	public abstract PState PressJump();
	public abstract PState ReleaseJump();
	public abstract PState HitWall(Vector2 wallCollisionVelocity, WallCollisionInfo collInfo);
	public abstract PState Brace();
	public abstract PState Grab();
	public abstract PState Interact();
	public abstract PState ToggleJournal();
	public abstract PState ToggleInventory();
	
	public void SetDirection(float horizontal){
		if(horizontal > 0){
			if(player.transform.eulerAngles.y == 180){
				player.transform.eulerAngles = new Vector2(0,0);
			}
			direction = 1;
		}
		if(horizontal < 0){
			if(player.transform.eulerAngles.y == 0){
				player.transform.eulerAngles = new Vector2(0,180);
			}
			direction = -1;
		}
	}

	public int GetDirection(){
		return direction;
	}
	
	protected void InverseDirectionCorrection(){
		if(rigidbody.velocity.x > 0){
			player.transform.eulerAngles = new Vector2(0,180);
		} else{
			player.transform.eulerAngles = new Vector2(0,0);
		}
	}
	
	protected void CancelBraceCooldown(){
		inputManager.CancelBraceCooldown();
	}
}
