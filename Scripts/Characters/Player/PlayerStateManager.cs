using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
	public PlayerHub player;
    PState currentState;
	
	void Start(){
		currentState = new PStateIdle(player, player.attributeManager.attrSet, player.rigidbody);
	}
	
	void Update(){
		currentState = currentState.Update();
	}
	
	void FixedUpdate(){
		currentState = currentState.FixedUpdate();
	}
	
	public void Move(float horizontal, float vertical){
		currentState = currentState.Move(horizontal, vertical);
	}
	
	public void HitGround(float hitForce){
		currentState = currentState.HitGround(hitForce);
	}
	
	public void PressJump(){
		currentState = currentState.PressJump();
	}
	
	public void ReleaseJump(){
		currentState = currentState.ReleaseJump();
	}
}
