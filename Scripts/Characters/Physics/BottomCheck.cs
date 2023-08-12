using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomCheck : MonoBehaviour
{
    public CharacterPhysicsChecker parentPhysics;
	public Rigidbody2D rigidbody;
	public float distanceToGround;
	public PlayerMover playerMover;
	LayerMask mask;
	List<Transform> collisions;
	
	void Start(){
		collisions = new List<Transform>();
		mask = LayerMask.GetMask("PhysicsEnvironment");
	}

    void OnCollisionEnter2D(Collision2D collision){
		RaycastHit2D hit = Physics2D.Raycast(rigidbody.position, Vector2.down, distanceToGround, mask);
		if(hit.collider != null && hit.transform == collision.transform){
			parentPhysics.isGrounded = true;
			parentPhysics.bottomCollisionSpeed = new Vector2(collision.relativeVelocity.x, collision.relativeVelocity.y);
			if(!collisions.Contains(collision.transform)){
				collisions.Add(collision.transform);
			}
			parentPhysics.stateManager.HitGround(collision.relativeVelocity.y);
		}
    }

    void OnCollisionStay2D(Collision2D collision){
        parentPhysics.isGrounded = true;
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
}
