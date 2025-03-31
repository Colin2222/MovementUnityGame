using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCheck : MonoBehaviour
{
    public CharacterPhysicsChecker parentPhysics;
	public Rigidbody2D rigidbody;
	public float distanceToWall;
	LayerMask mask;
	Collision2D lastFrontCollision;

	[System.NonSerialized]
	public WallCollisionInfo lastWallCollisionInfo;
	
	public Transform headCheck;
	public Transform middleUpperCheck;
	public Transform middleMiddleCheck;
	public Transform middleLowerCheck;
	public Transform feetCheck;
	public Transform faceplantCheck;
	public Transform lowerHeadCheck;
	
	public BoxCollider2D uprightHitbox;
	public BoxCollider2D flatHitbox;
	public BoxCollider2D halfHitbox;
	
	void Start(){
		mask = LayerMask.GetMask("PhysicsEnvironment");
	}

    void OnCollisionEnter2D(Collision2D collision){
		RaycastHit2D hitFeet = Physics2D.Raycast(feetCheck.position, Vector2.right * Mathf.Sign(-collision.relativeVelocity.x), distanceToWall, mask);
		RaycastHit2D hitUpperMiddle = Physics2D.Raycast(middleUpperCheck.position, Vector2.right * Mathf.Sign(-collision.relativeVelocity.x), distanceToWall, mask);
		RaycastHit2D hitMiddleMiddle = Physics2D.Raycast(middleMiddleCheck.position, Vector2.right * Mathf.Sign(-collision.relativeVelocity.x), distanceToWall, mask);
		RaycastHit2D hitLowerMiddle = Physics2D.Raycast(middleLowerCheck.position, Vector2.right * Mathf.Sign(-collision.relativeVelocity.x), distanceToWall, mask);
		RaycastHit2D hitHead = Physics2D.Raycast(headCheck.position, Vector2.right * Mathf.Sign(-collision.relativeVelocity.x), distanceToWall, mask);
		RaycastHit2D hitFaceplant = Physics2D.Raycast(faceplantCheck.position, Vector2.right * Mathf.Sign(-collision.relativeVelocity.x), distanceToWall, mask);
		RaycastHit2D hitLowerHead = Physics2D.Raycast(lowerHeadCheck.position, Vector2.right * Mathf.Sign(-collision.relativeVelocity.x), distanceToWall, mask);
		if(hitLowerMiddle.collider != null || hitUpperMiddle.collider != null || hitFeet.collider != null || hitHead.collider != null){
			parentPhysics.wallSide = Mathf.Sign(-collision.relativeVelocity.x);
			Vector2 normal = collision.GetContact(0).normal;
			if(normal.x != 0.0f){
				lastFrontCollision = collision;
				WallCollisionInfo collInfo = new WallCollisionInfo(hitHead.collider != null, hitUpperMiddle.collider != null, hitMiddleMiddle.collider != null, hitLowerMiddle.collider != null, hitFeet.collider != null, hitFaceplant.collider != null, hitLowerHead.collider != null);
				lastWallCollisionInfo = collInfo;
				parentPhysics.stateManager.HitWall(new Vector2(collision.relativeVelocity.x, collision.relativeVelocity.y), collInfo);
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
	
	public bool IsLowerContact(int direction){		
		RaycastHit2D hitLowerMiddle = Physics2D.Raycast(middleLowerCheck.position, Vector2.right * -direction, distanceToWall, mask);
		return hitLowerMiddle.collider != null;
	}

	public bool IsMiddleContact(int direction){		
		RaycastHit2D hitMiddleMiddle = Physics2D.Raycast(middleMiddleCheck.position, Vector2.right * -direction, distanceToWall, mask);
		return hitMiddleMiddle.collider != null;
	}
	
	public void SwitchHitboxes(int hitboxType){
		switch(hitboxType){
			case 1:
				uprightHitbox.enabled = true;
				flatHitbox.enabled = false;
				halfHitbox.enabled = false;
				break;
			case 2:
				uprightHitbox.enabled = false;
				flatHitbox.enabled = true;
				halfHitbox.enabled = false;
				break;
			case 3:
				uprightHitbox.enabled = false;
				flatHitbox.enabled = false;
				halfHitbox.enabled = true;
				break;
		}
	}
}

public class WallCollisionInfo{
	public bool touchHead;
	public bool touchUpperMiddle;
	public bool touchMiddleMiddle;
	public bool touchLowerMiddle;
	public bool touchFeet;
	public bool touchFaceplant;
	public bool touchLowerHead;

	public WallCollisionInfo(bool head, bool upperMiddle, bool middleMiddle, bool lowerMiddle, bool feet, bool faceplant, bool lowerHead){
		this.touchHead = head;
		this.touchUpperMiddle = upperMiddle;
		this.touchMiddleMiddle = middleMiddle;
		this.touchLowerMiddle = lowerMiddle;
		this.touchFeet = feet;
		this.touchFaceplant = faceplant;
		this.touchLowerHead = lowerHead;
	}
}