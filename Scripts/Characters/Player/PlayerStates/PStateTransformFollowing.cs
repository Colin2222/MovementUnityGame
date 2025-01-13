using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateTransformFollowing : PState
{
    Transform followTarget; 
    public PStateTransformFollowing(Transform followTarget){
        this.followTarget = followTarget;
        physics.ClearBottomCheck();
        player.transform.position = followTarget.position;
        player.transform.SetParent(followTarget, true);
		rigidbody.gravityScale = 0f;
        physics.gameObject.SetActive(false);
        rigidbody.bodyType = RigidbodyType2D.Static;
        player.animator.Play("PlayerIdle");
	}
	
    public override PState Update(){
		return this;
	}
	
	public override PState FixedUpdate(){
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
