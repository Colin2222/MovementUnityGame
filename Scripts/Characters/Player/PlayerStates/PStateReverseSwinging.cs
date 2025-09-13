using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateReverseSwinging : PState
{
    float swingingTimer;
    float layerChangeTime;
    bool layerChanged = false;
    float exitSpeed;
    float entrySpeed;
    Transform reversiblePoint;
    SpriteRenderer reversibleSprite;
    public PStateReverseSwinging(float entrySpeed)
    {
        swingingTimer = attr.reverseSwingTime;
        layerChangeTime = swingingTimer - attr.reverseLayerChangeTime;
        this.entrySpeed = entrySpeed;
        exitSpeed = -entrySpeed;
        rigidbody.velocity = new Vector2(0f, 0f);
        reversiblePoint = player.reverseHandler.GetLastReversible();
        reversibleSprite = reversiblePoint.GetComponent<ReversibleScript>().spriteRenderer;
        player.transform.position = new Vector3(reversiblePoint.position.x + (attr.reverseXOffset * (entrySpeed > 0 ? 1 : -1)), player.transform.position.y, 0f);
        player.animator.Play("PlayerReverseSwinging");
        layerChanged = false;
    }

    public override PState Update()
    {
        swingingTimer -= Time.deltaTime;
        if (swingingTimer <= 0)
        {
            player.rigidbody.velocity = new Vector2(exitSpeed, 0f);
            base.SetDirection(exitSpeed > 0 ? 1 : -1);
            player.reverseHandler.ResetReversibleReference();
            player.animator.Play("PlayerRunningFast");
            reversibleSprite.sortingOrder = -2;
            player.transform.position = new Vector3(reversiblePoint.position.x + (attr.reverseXExitOffset * (entrySpeed > 0 ? 1 : -1)), player.transform.position.y, 0f);
            return new PStateMoving();
        }
        else if (!layerChanged && swingingTimer <= layerChangeTime)
        {
            reversibleSprite.sortingOrder = 0;
            layerChanged = true;
        }
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

	public override PState Grab(){
		return this;
	}
	
	public override PState LeaveGround(){
		return new PStateSoaring();
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
