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
		currentState = new PStateIdle(player, player.inputManager, player.attributeManager.attrSet, player.rigidbody, player.physics);
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
	
	public void HitGround(float hitSpeedX, float hitSpeedY, bool stayCollision){
		// add an option for the weird stay collisions, only triggers in the states where it gets weird
		if(stayCollision){
			if(currentState is PStateFaceplantSoaring || currentState is PStateHeadHitSoaring){
				currentState = currentState.HitGround(hitSpeedX, hitSpeedY);
			}
		} else{
			currentState = currentState.HitGround(hitSpeedX, hitSpeedY);
		}
	}
	
	public void HitWall(Vector2 wallCollisionVelocity, WallCollisionInfo collInfo){
		currentState = currentState.HitWall(wallCollisionVelocity, collInfo);
	}
	
	public void LeaveGround(){
		currentState = currentState.LeaveGround();
	}
	
	public void LeaveWall(){
		currentState = currentState.LeaveWall();
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
	
	public bool ToggleJournal(){
		currentState = currentState.ToggleJournal();
		return (currentState is PStateViewingJournal);
	}
}
