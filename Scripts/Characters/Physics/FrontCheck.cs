using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCheck : MonoBehaviour
{
    public CharacterPhysicsChecker parentPhysics;

    void OnCollisionEnter2D(Collision2D collision){
        parentPhysics.isWalled = true;
		parentPhysics.frontCollisionSpeed = collision.relativeVelocity.x;
    }

    void OnCollisionStay2D(Collision2D collision){
        parentPhysics.isWalled = true;
		parentPhysics.frontCollisionSpeed = 0.0f;
    }

    void OnCollisionExit2D(Collision2D collision){
        parentPhysics.isWalled = false;
    }
}
