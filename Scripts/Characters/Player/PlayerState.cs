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
	public bool isRunJumping = false;
	public bool isJumping = false;
	public bool isExtraJumping = false;
	public bool isJumpBracing = false;
	public bool isFalling = false;
	
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
		isExtraJumping = false;
		isFalling = false;
	}
}
