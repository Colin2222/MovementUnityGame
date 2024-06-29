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
	public float stillJumpMinimumBraceRatio;
	public float jumpForce;
	public float braceTime;
	public float braceCooldownTime;
	public float gravityScale;
	public float cornerMantleTime;
	public float cornerClimbTime;
	public float cornerClimbVertJoystickThreshold;
	public float cornerTripSpeedLoss;
	public float cornerTripTime;
	public float cornerTripMinimumSpeed;
	public float cornerStunReboundMinSpeed;
	public float cornerStunSlideCoefficient;
	public float cornerStunGetupStartSpeed;
	public float cornerStunGetupWaitTime;
	public float cornerStunGetupTime;
	public float cornerVaultTime;
	public float cornerMantleMinHorizontalKeepSpeed;
	public float wallBraceTime;
	public float wallPushTime;
	public float wallPushHorizontalRetention;
	public float wallPushBoost;
	public float wallLaunchTime;
	public float wallLaunchHorizontalRetention;
	public float wallLaunchMinimumHorizontal;
	public float wallLaunchBoost;
	public float wallLaunchMaxVerticalSpeed;
	public float wallSlideUpwardsCoefficient;
	public float wallSlideDownwardsCoefficient;
	public float wallPushLaunchCoefficient;
	public float wallPushToLaunchHorizontalRetention;
	public float wallPushToLaunchBoost;
	public float wallSplatMinSpeed;
	public float wallSplatStickTime;
	public float wallSplatStumbleTime;
	public float wallSplatStumbleSpeed;
	
	public float groundHitSpeedRollThreshold;
	public float groundHitSpeedRollMin;
	public float groundHitSpeedMaxThreshold;
	public float groundRollBraceWindow;
	public float groundRollTime;
	public float groundRollMinSpeed;
	public float groundRollJumpQueueTime;
	public float landingBigTime;
	public float landingBigForceMultiplier;
	public float optionalRollWindow;
}
