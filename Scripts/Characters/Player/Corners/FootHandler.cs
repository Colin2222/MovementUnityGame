using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootHandler : MonoBehaviour
{
    public CornerHandler cornerHandler;
	public Transform checkingTrans;
	public Transform groundCheckTrans;
	public float checkDistance;
	public float groundCheckDistance;
	
	LayerMask cornerMask;
	LayerMask groundMask;
	
    void OnTriggerEnter2D(Collider2D col){
		cornerHandler.footCorner = col.gameObject.transform;
	}
	
	void OnTriggerExit2D(Collider2D col){
		cornerHandler.footCorner = null;
	}
	
	void Start(){
		cornerMask = LayerMask.GetMask("Corners");
		groundMask = LayerMask.GetMask("PhysicsEnvironment");
	}
	
	public bool CheckForCorner(int direction){
		RaycastHit2D hitFeet = Physics2D.Raycast(checkingTrans.position, Vector2.right * direction, checkDistance, cornerMask);
		if(hitFeet.collider != null){
			RaycastHit2D groundCheck = Physics2D.Raycast(groundCheckTrans.position, Vector2.down, groundCheckDistance, groundMask);
			if(groundCheck.collider != null){
				cornerHandler.footCorner = hitFeet.collider.transform;
				return true;
			}
		}
		return false;
	}
}
