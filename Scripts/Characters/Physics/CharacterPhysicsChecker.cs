using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPhysicsChecker : MonoBehaviour
{
	public PlayerStateManager stateManager;
	public BottomCheck bottomCheck;
	public FrontCheck frontCheck;
	
    [System.NonSerialized]
    public bool isGrounded = false;
    [System.NonSerialized]
    public bool topTouch = false;
    [System.NonSerialized]
    public bool isWalled = false;
    [System.NonSerialized]
    public bool backTouch = false;
	[System.NonSerialized]
	public Vector2 frontCollisionSpeed;
	[System.NonSerialized]
	public Vector2 bottomCollisionSpeed;
	[System.NonSerialized]
	public Vector2 lastBottomCollisionSpeed;
	[System.NonSerialized]
    public float wallSide = 1;

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
	
	public void ClearBottomCheck(){
		bottomCheck.ClearReferences();
		isGrounded = false;
	}
	
	// 1 == upright/normal
	// 2 == laying down
	public void SwitchHitboxes(int hitboxType){
		bottomCheck.SwitchHitboxes(hitboxType);
		frontCheck.SwitchHitboxes(hitboxType);
	}
}
