using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomCheck : MonoBehaviour
{
    public CharacterPhysicsChecker parentPhysics;
	public Rigidbody2D rigidbody;
	public float distanceToGround;
	LayerMask mask;
	List<Transform> collisions;
	
	void Start(){
		collisions = new List<Transform>();
		mask = LayerMask.GetMask("PhysicsEnvironment");
	}

    void OnCollisionEnter2D(Collision2D collision){
		if(collision.GetContact(0).normal.y == 1.0f){
			if(!collisions.Contains(collision.transform)){
				collisions.Add(collision.transform);
			}
			
			if(!parentPhysics.isGrounded){
				parentPhysics.isGrounded = true;
				parentPhysics.bottomCollisionSpeed = new Vector2(collision.relativeVelocity.x, collision.relativeVelocity.y);
				parentPhysics.lastBottomCollisionSpeed = parentPhysics.bottomCollisionSpeed;
				parentPhysics.stateManager.HitGround(collision.relativeVelocity.x, collision.relativeVelocity.y);
			}
		}
    }

    void OnCollisionStay2D(Collision2D collision){
		if(!collisions.Contains(collision.transform)){
			collisions.Add(collision.transform);
		}
		parentPhysics.bottomCollisionSpeed = new Vector2(0.0f, 0.0f);
    }

    void OnCollisionExit2D(Collision2D collision){
		if(collisions.Contains(collision.transform)){
			collisions.Remove(collision.transform);
		}
		
		if(collisions.Count == 0){
			parentPhysics.isGrounded = false;
			parentPhysics.stateManager.LeaveGround();
		}
    }
	
	public void ClearReferences(){
		collisions.Clear();
	}
}
