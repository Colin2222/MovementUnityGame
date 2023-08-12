using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
	public PlayerHub player;
    PState currentState;
	
	public Type GetStateType(){
		return currentState.GetType();
	}
	
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
	
	public void LeaveGround(){
		currentState = currentState.LeaveGround();
	}
	
	public void PressJump(){
		currentState = currentState.PressJump();
	}
	
	public void ReleaseJump(){
		currentState = currentState.ReleaseJump();
	}
	
	public bool Brace(){
		PState oldState = currentState;
		currentState = currentState.Brace();
		return oldState != currentState;
	}
	
	public void ClimbUp(){
		currentState = currentState.ClimbUp();
	}
	
	public void ClimbDown(){
		currentState = currentState.ClimbDown();
	}
}
