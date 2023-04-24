using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCheck : MonoBehaviour
{
    public CharacterPhysicsChecker parentPhysics;
	public Rigidbody2D rigidbody;
	public float distanceToWall;
	LayerMask mask;
	
	void Start(){
		mask = LayerMask.GetMask("PhysicsEnvironment");
	}

    void OnCollisionEnter2D(Collision2D collision){
		RaycastHit2D hit = Physics2D.Raycast(rigidbody.position, Vector2.right * Mathf.Sign(-collision.relativeVelocity.x), distanceToWall, mask);
		if(hit.collider != null){
			parentPhysics.isWalled = true;
			parentPhysics.frontCollisionSpeed = collision.relativeVelocity.x;
		}
    }

    void OnCollisionStay2D(Collision2D collision){
        parentPhysics.isWalled = true;
		parentPhysics.frontCollisionSpeed = 0.0f;
    }

    void OnCollisionExit2D(Collision2D collision){
        parentPhysics.isWalled = false;
    }
}
