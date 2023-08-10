using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateSoaring : PState
{
    public PStateSoaring(){
		
	}
	
    public override PState Update(){
		return this;
	}
	
	public override PState FixedUpdate(){
		return this;
	}
	
    public override PState HitGround(float hitSpeed){
		return new PStateMoving();
	}
	
	public override PState Move(float horizontal, float vertical){
		return this;
	}
	
	public override PState ClimbUp(){
		return this;
	}
	
	public override PState ClimbDown(){
		return this;
	}
	
	public override PState PressJump(){
		return this;
	}
	
	public override PState ReleaseJump(){
		return this;
	}
	
	public override PState HitWall(){
		return this;
	}
	
	public override PState PressBrace(){
		return this;
	}
	
	public override PState LeaveGround(){
		return this;
	}
}
