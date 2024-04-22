using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCheck : MonoBehaviour
{
    public CharacterPhysicsChecker parentPhysics;
	public Rigidbody2D rigidbody;
	public float distanceToWall;
	LayerMask mask;
	
	public Transform headCheck;
	public Transform middleUpperCheck;
	public Transform middleLowerCheck;
	public Transform feetCheck;
	
	void Start(){
		mask = LayerMask.GetMask("PhysicsEnvironment");
	}

    void OnCollisionEnter2D(Collision2D collision){
		RaycastHit2D hitFeet = Physics2D.Raycast(feetCheck.position, Vector2.right * Mathf.Sign(-collision.relativeVelocity.x), distanceToWall, mask);
		RaycastHit2D hitUpperMiddle = Physics2D.Raycast(middleUpperCheck.position, Vector2.right * Mathf.Sign(-collision.relativeVelocity.x), distanceToWall, mask);
		RaycastHit2D hitLowerMiddle = Physics2D.Raycast(middleLowerCheck.position, Vector2.right * Mathf.Sign(-collision.relativeVelocity.x), distanceToWall, mask);
		RaycastHit2D hitHead = Physics2D.Raycast(headCheck.position, Vector2.right * Mathf.Sign(-collision.relativeVelocity.x), distanceToWall, mask);
		if(hitLowerMiddle.collider != null || hitUpperMiddle.collider != null || hitFeet.collider != null || hitHead.collider != null){
			parentPhysics.wallSide = Mathf.Sign(-collision.relativeVelocity.x);
			Vector2 normal = collision.GetContact(0).normal;
			if(normal.x != 0.0f){
				parentPhysics.stateManager.HitWall(new Vector2(collision.relativeVelocity.x, collision.relativeVelocity.y), new WallCollisionInfo(hitHead.collider != null, hitUpperMiddle.collider != null, hitLowerMiddle.collider != null, hitFeet.collider != null));
			}
			/*
			if(!parentPhysics.isWalled && false){
				Debug.Log("HIT");
				RaycastHit2D hitHead = Physics2D.Raycast(headCheck.position, Vector2.right * Mathf.Sign(-collision.relativeVelocity.x), distanceToWall, mask);
				RaycastHit2D hitUpperMiddle = Physics2D.Raycast(middleUpperCheck.position, Vector2.right * Mathf.Sign(-collision.relativeVelocity.x), distanceToWall, mask);
				RaycastHit2D hitLowerMiddle = Physics2D.Raycast(middleLowerCheck.position, Vector2.right * Mathf.Sign(-collision.relativeVelocity.x), distanceToWall, mask);
				RaycastHit2D hitFeet = Physics2D.Raycast(feetCheck.position, Vector2.right * Mathf.Sign(-collision.relativeVelocity.x), distanceToWall, mask);
				parentPhysics.stateManager.HitWall(new Vector2(collision.relativeVelocity.x, collision.relativeVelocity.y), new WallCollisionInfo(hitHead.collider == null, hitUpperMiddle.collider == null, hitLowerMiddle.collider == null, hitFeet.collider == null));
			}
			*/
			parentPhysics.isWalled = true;
			parentPhysics.frontCollisionSpeed = new Vector2(collision.relativeVelocity.x, collision.relativeVelocity.y);
		}
    }

    void OnCollisionStay2D(Collision2D collision){
        parentPhysics.isWalled = true;
		parentPhysics.frontCollisionSpeed = new Vector2(0.0f, 0.0f);
    }

    void OnCollisionExit2D(Collision2D collision){
        parentPhysics.isWalled = false;
		parentPhysics.stateManager.LeaveWall();
    }
}

public class WallCollisionInfo{
	public bool touchHead;
	public bool touchUpperMiddle;
	public bool touchLowerMiddle;
	public bool touchFeet;
	
	public WallCollisionInfo(bool head, bool upperMiddle, bool lowerMiddle, bool feet){
		this.touchHead = head;
		this.touchUpperMiddle = upperMiddle;
		this.touchLowerMiddle = lowerMiddle;
		this.touchFeet = feet;
	}
}
