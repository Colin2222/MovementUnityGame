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
	
	// not in reference to player, in reference to corner player will potentially be climbing down
	public float smallCornerCheckVerticalOffset;
	public float smallCornerCheckHorizontalOffset;
	public float smallCornerCheckDistance;
	
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
		// check there is a corner the player can climb down
		RaycastHit2D hitFeet = Physics2D.Raycast(checkingTrans.position, Vector2.right * direction, checkDistance, cornerMask);
		if(hitFeet.collider != null){
			//check the player is facing the right way to climb down the corner
			RaycastHit2D groundCheck = Physics2D.Raycast(groundCheckTrans.position, Vector2.down, groundCheckDistance, groundMask);
			if(groundCheck.collider != null){
				//check the player wouldnt be climbing down a small ledge and clip into the world after climbing down
				Vector2 cornerCheckPos = new Vector2(hitFeet.collider.transform.position.x + (smallCornerCheckHorizontalOffset * direction), hitFeet.collider.transform.position.y - smallCornerCheckVerticalOffset);
				RaycastHit2D smallCornerCheck = Physics2D.Raycast(cornerCheckPos, Vector2.down, smallCornerCheckDistance, groundMask);
				if (smallCornerCheck.collider == null)
				{
					cornerHandler.footCorner = hitFeet.collider.transform;
					return true;
				}
			}
		}
		return false;
	}
}
