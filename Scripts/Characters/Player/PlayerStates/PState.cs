using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PState
{
	protected static PlayerHub player;
	protected static PlayerAttributeSet attr;
	protected static Rigidbody2D rigidbody;
	protected static int direction;
	
	public abstract PState Update();
	public abstract PState FixedUpdate();
	public abstract PState Move(float horizontal, float vertical);
	public abstract PState HitGround(float hitSpeed);
	public abstract PState LeaveGround();
	public abstract PState ClimbUp();
	public abstract PState ClimbDown();
	public abstract PState PressJump();
	public abstract PState ReleaseJump();
	public abstract PState HitWall(Vector2 wallCollisionVelocity);
	public abstract PState Brace();
	
	protected void SetDirection(float horizontal){
		if(horizontal > 0){
			if(direction != 1){
				player.transform.eulerAngles = new Vector2(0,0);
			}
			direction = 1;
		}
		if(horizontal < 0){
			if(direction != -1){
				player.transform.eulerAngles = new Vector2(0,180);
			}
			direction = -1;
		}
	}
	
	protected void InverseDirectionCorrection(){
		if(rigidbody.velocity.x > 0){
			player.transform.eulerAngles = new Vector2(0,180);
		} else{
			player.transform.eulerAngles = new Vector2(0,0);
		}
	}
}
