using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomCheck : MonoBehaviour
{
    public CharacterPhysicsChecker parentPhysics;
	public Rigidbody2D rigidbody;
	public float distanceToGround;
	LayerMask mask;
	
	void Start(){
		mask = LayerMask.GetMask("PhysicsEnvironment");
		Debug.Log(mask.value);
	}

    void OnCollisionEnter2D(Collision2D collision){
		RaycastHit2D hit = Physics2D.Raycast(rigidbody.position, Vector2.down, distanceToGround + 0.1f, mask);
		if(hit.collider != null){
			Debug.Log(hit.transform.name);
			parentPhysics.isGrounded = true;
			parentPhysics.bottomCollisionSpeed = collision.relativeVelocity.y;
		}
    }

    void OnCollisionStay2D(Collision2D collision){
        parentPhysics.isGrounded = true;
		parentPhysics.bottomCollisionSpeed = 0.0f;
    }

    void OnCollisionExit2D(Collision2D collision){
        parentPhysics.isGrounded = false;
    }
}
