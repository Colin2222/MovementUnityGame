using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAttributeSet
{
    public float moveForce;
	public float maxRunSpeed;
	public float slideForceMultiplier;
	public float slideStopSpeedTarget;
	public float runningJumpSpeed;
	public float jumpBraceTime;
	public float maxStillJumpAngleFromYAxis;
	public float stillJumpLaunchTime;
	public float jumpForce;
	public float braceTime;
	public float braceCooldownTime;
	public float gravityScale;
	public float cornerMantleTime;
	public float cornerClimbTime;
	public float cornerClimbVertJoystickThreshold;
}