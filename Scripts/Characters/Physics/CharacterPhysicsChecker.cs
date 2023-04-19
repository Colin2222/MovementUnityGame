using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPhysicsChecker : MonoBehaviour
{
    [System.NonSerialized]
    public bool isGrounded = false;
    [System.NonSerialized]
    public bool topTouch = false;
    [System.NonSerialized]
    public bool isWalled = false;
    [System.NonSerialized]
    public bool backTouch = false;
	[System.NonSerialized]
	public float frontCollisionSpeed;
	[System.NonSerialized]
	public float bottomCollisionSpeed;

    void OnCollisionEnter2D(Collision2D collision){
        LayerMask layer = collision.gameObject.layer;
        Vector3 otherPos = collision.transform.position;
    }

    void OnCollisionStay2D(Collision2D collision){
        LayerMask layer = collision.gameObject.layer;
        Vector3 otherPos = collision.transform.position;
    }

    void OnCollisionExit2D(Collision2D collision){
        LayerMask layer = collision.gameObject.layer;
        Vector3 otherPos = collision.transform.position;
    }
}
