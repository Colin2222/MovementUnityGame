using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public bool isRunning = false;
	public bool isFullRunning = false;
	public bool isStanding = true;
	public bool isSlideStopping = false;
	public bool isSlideTurning = false;
	public bool isWallSplatting = false;
	public bool isWallSplatStumbling = false;
	public bool isWalking = false;
	
	public bool isJumpBracing = false;
	public bool isStillJumpLaunching = false;
	
	// general isJumping state, with all subsequent specific types of jumping
	public bool isJumping = false;
	public bool isRunJumping = false;
	public bool isStillJumping = false;
	
	// wall collision states
	public bool isWallColliding = false;
	public bool isWallBracing = false;
	public bool isAirWallSplatting = false;
	public bool isWallLaunching = false; 
	public bool isWallPushing = false;
	
	public bool isStillLandingSmall = false;
	public bool isStillLandingBig = false;
	public bool isStillLanding = false;
	public bool isFalling = false;
	
	// corner grabbing states
	public bool isCornerGrabbing = false; 
	public bool isCornerClimbing = false; 
	
	// inventory states
	public bool isInInventory = false;
	public bool isInventoryExiting = false; 
	public bool isHoldingItem = false;
	
	public bool isBracing = false;
	public int direction = 1;
	
	public PlayerState(){
		
	}
	
	public void SweepFalse(){
		isRunning = false;
		isFullRunning = false;
		isStanding = false;
		isSlideStopping = false;
		isSlideTurning = false;
		isWallSplatting = false;
		isWallSplatStumbling = false;
		isWalking = false;
		isRunJumping = false;
		isJumping = false;
		isJumpBracing = false;
		isStillJumping = false;
		isStillJumpLaunching = false;
		isStillLandingSmall = false;
		isStillLandingBig = false;
		isStillLanding = false;
		isFalling = false;
		isBracing = false;
		isWallColliding = false;
		isWallBracing = false;
		isAirWallSplatting = false;
		isWallLaunching = false;
		isWallPushing = false; 
		isCornerGrabbing = false;
		isCornerClimbing = false;
		isInInventory = false;
		isInventoryExiting = false;
	}
}
