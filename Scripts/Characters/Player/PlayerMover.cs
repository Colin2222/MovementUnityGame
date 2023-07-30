using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
	public PlayerHub player;
	public CornerHandler cornerHandler;
	public PlayerInventoryHandler inventoryHandler;
	public PlayerInteractor interactor;
	Rigidbody2D rigidbody;
	PlayerState state;
	Animator animator;
	
	// used to prevent movement in the middle of certain moves (eg prevent changing directions while in the air)
	bool movementLocked = false;
	// used to prevent movement when in a cutscene or in inventory menu
	bool physControlLocked = false; 
	
	// ground movement variables
	public float speed;
	public float moveForce;
	public float maxRunSpeed;
	public float walkJoystickThreshold;
	public float slideStopSpeedTarget;
	public float slideForceMultiplier;
	
	// wall splat variables
	public float wallSplatStickTime;
	float wallSplatStickTimer;
	int wallSplatSide;
	public float wallSplatStumbleTime;
	public float wallSplatStumbleSpeed;
	float wallSplatStumbleTimer;
	public float wallSplatMinSpeed;
	
	// wall collision variables
	public float wallSlideUpwardsCoefficient;
	public float wallSlideDownwardsCoefficient;
	public float wallLaunchCoefficient;
	public float wallBraceTime;
	public float wallAirSplatMinSpeed;
	float wallBraceTimer;
	float wallCollisionEntranceSpeed;
	public float wallLaunchTime;
	float wallLaunchTimer;
	public float wallPushTime;
	float wallPushTimer;
	Vector2 wallCollisionVelocity;
	bool wallPushed = false;
	bool wallLaunched = false; 
	public float wallLaunchHorizontalRetention;
	public float wallLaunchMinimumHorizontal;
	public float wallPushHorizontalRetention;
	public float wallLaunchBoost;
	public float wallPushBoost;
	public float wallLaunchMaxVerticalSpeed;
	
	
	// bracing variables
	public float braceTime;
	public float braceCooldownTime;
	float braceTimer = 0.0f;
	float braceCooldownTimer = 0.0f;
	
	// jumping variables
	public float jumpForce;
	public float jumpForgivenessTime;
    [System.NonSerialized]
    public float timeSincePressed;
	float timeSinceGrounded;
	public float coyoteTime;
	bool jumped = false;
	bool stillJumped = false;
	float jumpTimeCounter;
    public float jumpTime;
	public float runningJumpSpeed;
	float jumpBraceCounter;
	public float jumpBraceTime;
	public float jumpBraceStartingTime;
	public float jumpLaunchTime;
	float jumpLaunchTimer;
	public float minimumJumpBraceRatio;
	public float stillLandLittleTime;
	public float stillLandBigTime;
	float stillLandTimer;
	public float stillLandSmallMinSpeed;
	public float maxStillJumpAngleFromYAxis;
	float aimAngle;
	
	// corner grabbing variables
	float gravityCache;
	int cornerDir;
	public float cornerClimbTime;
	public float cornerMantleTime;
	float cornerClimbTimer;
	bool cornerClimbEnding = false;
	public float cornerClimbVertJoystickThreshold;
	bool climbJoystickTooFar = false; 
	
	private float horizontal = 0;
    private float vertical = 0;
	
	// inventory variables
	public float inventoryExitTime;
	float inventoryExitTimer;
	
	
	
	// button pressing
    private bool jumpPressed = false;
    private bool jumpJustPressed = false;
	private bool bracePressed = false;
	private bool braceJustPressed = false;
	private bool inventoryPressed = false;
	private bool inventoryJustPressed = false;
	private bool itemGrabPressed = false;
	private bool itemGrabJustPressed = false;
	private bool menuUpPressed = false;
	private bool menuUpJustPressed = false;
	private bool menuDownPressed = false;
	private bool menuDownJustPressed = false;
	private bool menuRightPressed = false;
	private bool menuRightJustPressed = false;
	private bool menuLeftPressed = false;
	private bool menuLeftJustPressed = false;
	private bool menuPageRightPressed = false;
	private bool menuPageRightJustPressed = false;
	private bool menuPageLeftPressed = false;
	private bool menuPageLeftJustPressed = false;
	private bool interactPressed = false;
	private bool interactJustPressed = false;

	
    // Start is called before the first frame update
    void Start() 
    {
        rigidbody = player.rigidbody;
		state = player.state;
		animator = player.animator;
		
		//Time.timeScale = 0.1f;
		//Time.timeScale = 0.3f;
		//Time.timeScale = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {	
		if(physControlLocked){
			jumpJustPressed = false;
			braceJustPressed = false;
			inventoryJustPressed = false;
			itemGrabJustPressed = false;
			menuUpJustPressed = false;
			menuDownJustPressed = false;
			menuRightJustPressed = false;
			menuLeftJustPressed = false;
			menuPageRightJustPressed = false;
			menuPageLeftJustPressed = false;
			interactJustPressed = false;
			vertical = 0;
			horizontal = 0;
		}
	
		HandleBracing();
		HandleWallBracing();
		HandleWallLaunching();
		HandleWallPushing();
		HandleWallSplatSticking();
		HandleWallSplatStumbling();
		HandleJumping();
		HandleJumpBracing();
		HandleStillLanding();
		HandleCornerGrabbing();
		HandleItemGrabbing();
		HandleInteraction();
		HandleMoveInput();
		
		HandleInventory();
		
		jumpJustPressed = false;
		braceJustPressed = false;
		inventoryJustPressed = false;
		itemGrabJustPressed = false;
		menuUpJustPressed = false;
		menuDownJustPressed = false;
		menuRightJustPressed = false;
		menuLeftJustPressed = false;
		menuPageRightJustPressed = false;
		menuPageLeftJustPressed = false;
		interactJustPressed = false;
    }
	
	void FixedUpdate(){
		if(!state.isCornerGrabbing && !state.isCornerClimbing && !state.isCornerMantling){
			HandleJumpingPhysics();
				
				
			HandleWallColliding();
			HandleWallCollidingPhysics();
			
			HandleMovePhysics();
			HandleWallSplatting();
			
			CheckGroundedness();
		} else{
			if(cornerClimbEnding){
				cornerClimbEnding = false;
				if(cornerHandler.corner != null){
					transform.parent.position = new Vector3(cornerHandler.corner.position.x + (cornerHandler.cornerEndClimbOffsetX * cornerDir * -1), cornerHandler.corner.position.y + cornerHandler.cornerEndClimbOffsetY, 0);
				} else if(cornerHandler.mantleCorner != null){
					transform.parent.position = new Vector3(cornerHandler.mantleCorner.position.x + (cornerHandler.cornerEndClimbOffsetX * cornerDir * -1), cornerHandler.mantleCorner.position.y + cornerHandler.cornerEndClimbOffsetY, 0);
				} else{
					transform.parent.position = new Vector3(cornerHandler.lastCorner.position.x + (cornerHandler.cornerEndClimbOffsetX * cornerDir * -1), cornerHandler.lastCorner.position.y + cornerHandler.cornerEndClimbOffsetY, 0);
				}
				rigidbody.gravityScale = gravityCache;
				movementLocked = false;
				state.isCornerClimbing = false;
				state.isCornerMantling = false;
				state.isStanding = true;
				animator.Play("PlayerIdle");
				state.direction = cornerDir;
				climbJoystickTooFar = (Mathf.Abs(vertical) > cornerClimbVertJoystickThreshold);
			}
		}
	}
	
	void CheckGroundedness(){
		// update coyote time and jump forgiveness time
		if(player.physics.isGrounded){
            timeSinceGrounded = 0.0f;
        } else{
			if(timeSinceGrounded == 0.0f){
				// custom logic for running off edges
				if(state.isRunning || state.isSlideStopping || state.isSlideTurning){
					state.SweepFalse();
					state.isStillJumping = true;
					state.isJumping = true;
					animator.Play("PlayerSoaringStill");
				}
			}
			
            timeSinceGrounded += Time.fixedDeltaTime;
		}
		if(timeSincePressed < jumpForgivenessTime){
            timeSincePressed += Time.fixedDeltaTime;
        }
	}
	
	public void HitGround(){
		if(state.isWallColliding || state.isWallBracing || state.isWallPushing || state.isWallLaunching){
			if(player.physics.bottomCollisionSpeed.y > stillLandSmallMinSpeed){
				state.SweepFalse();
				state.isStillLandingSmall = true;
				state.isStillLanding = true;
				animator.Play("PlayerLandingStillSmall");
				stillLandTimer = stillLandLittleTime;
			} else{
				state.SweepFalse();
				state.isStanding = true;
				animator.Play("PlayerIdle");
			}
			movementLocked = false;
		}
	}
	
	void HandleInventory(){
		if(inventoryJustPressed){
			state.SweepFalse();
			state.isInInventory = inventoryHandler.ToggleInventory();
			
			if(!state.isInInventory){
				state.isInventoryExiting = true;
				inventoryExitTimer = inventoryExitTime;
				animator.Play("PlayerBagExiting");
			} else{
				physControlLocked = true;
				animator.Play("PlayerBagReaching");
			}
		}
		
		if(state.isInInventory){
			if(menuUpJustPressed){
				inventoryHandler.MoveUp();
			} else if(menuDownJustPressed){
				inventoryHandler.MoveDown();
			} else if(menuRightJustPressed){
				inventoryHandler.MoveRight();
			} else if(menuLeftJustPressed){
				inventoryHandler.MoveLeft();
			}
			
			if(menuPageRightJustPressed){
				inventoryHandler.PageRight();
			} else if(menuPageLeftJustPressed){
				inventoryHandler.PageLeft();
			}
		}
		
		if(state.isInventoryExiting){
			inventoryExitTimer -= Time.deltaTime;
			if(inventoryExitTimer <= 0){
				state.isInventoryExiting = false;
				state.isStanding = true;
				physControlLocked = false;
			}
		}
	}
	
	void HandleInteraction(){
		if(interactJustPressed){
			interactor.Interact();
		}
	}
	
	void HandleBracing(){
		//Debug.Log(state.isBracing);
		if(state.isBracing){
			braceTimer -= Time.deltaTime;
			if(braceTimer <= 0.0f || !bracePressed){
				state.isBracing = false;
				braceCooldownTimer = braceCooldownTime;
			}
		} else{
			if(braceCooldownTimer > 0.0f){
				braceCooldownTimer -= Time.deltaTime;
			} else{
				if(braceJustPressed){
					state.isBracing = true;
					braceTimer = braceTime;
				}
			}
		}
	}
	
	void HandleStillLanding(){
		if(state.isStillLandingSmall || state.isStillLandingBig){
			movementLocked = true;
			stillLandTimer -= Time.deltaTime;
			if(stillLandTimer <= 0.0f){
				state.isStillLanding = false;
				state.isStillLandingBig = false;
				state.isStillLandingSmall = false;
				animator.Play("PlayerIdle");
				state.isStanding = true;
				movementLocked = false;
			}
		}
	}
	
	void HandleJumping(){
        // basic jump off ground (including coyote time)
        if(jumpJustPressed && !movementLocked && player.physics.isGrounded && !state.isJumping){
			// handle running jumps
			if(!state.isSlideTurning && !state.isSlideStopping && Mathf.Abs(rigidbody.velocity.x) > runningJumpSpeed){ 
				animator.Play("PlayerJumpingRunning");
				jumped = true;
				state.SweepFalse();
				state.isJumping = true;
				movementLocked = true;
				state.isRunJumping = true;
				jumpTimeCounter = jumpTime;
				rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
			} else if(Mathf.Abs(rigidbody.velocity.x) <= runningJumpSpeed){
				state.SweepFalse();
				state.isJumpBracing = true;
				animator.Play("PlayerJumpBracing");
				jumpBraceCounter = jumpBraceStartingTime;
			}
        }
	}
	
	void HandleJumpingPhysics(){
		if(jumped){
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
			rigidbody.AddForce(new Vector2(0,jumpForce), ForceMode2D.Impulse);
			jumped = false;
		} else if(stillJumped){
			rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
			float jumpForceMultiplier = Mathf.Clamp(jumpBraceCounter, 0.0f, jumpBraceTime) / jumpBraceTime;
			
			float originalAngle = aimAngle;
			
			if(aimAngle == 0.0f){
				aimAngle = 1.5707f;
			}
			if(state.direction == 1){
				if(aimAngle >= 0){
					if(aimAngle < 1.5707f - maxStillJumpAngleFromYAxis){
						aimAngle = 1.5707f - maxStillJumpAngleFromYAxis;
					} else if (aimAngle > 1.5707f){
						aimAngle = 1.5707f;
					}
				} else{
					if(aimAngle > -.785f){
						aimAngle = 1.5707f - maxStillJumpAngleFromYAxis;
					} else{
						aimAngle = 1.5707f;
					}
				}
			} else if(state.direction == -1){
				if(aimAngle >= 0){
					if(aimAngle > 1.5707f + maxStillJumpAngleFromYAxis){
						aimAngle = 1.5707f + maxStillJumpAngleFromYAxis;
					} else if(aimAngle < 1.5707f){
						aimAngle = 1.5707f;
					}
				} else{
					if(aimAngle < -2.356f){
						aimAngle = 1.5707f + maxStillJumpAngleFromYAxis;
					} else{
						aimAngle = 1.5707f;
					}
				}
			}
			
			
			//Debug.Log("horizontal: " + horizontal + " vertical: " + vertical + " initangle: " + originalAngle + " angle: " + aimAngle);
			
			rigidbody.AddForce(new Vector2(jumpForce * Mathf.Cos(aimAngle) * jumpForceMultiplier, jumpForce * Mathf.Sin(aimAngle) * jumpForceMultiplier), ForceMode2D.Impulse);
			stillJumped = false;
        } else if(state.isJumping && player.physics.isGrounded && timeSinceGrounded > 0.0f){
			if(state.isStillJumping && player.physics.bottomCollisionSpeed.y > stillLandSmallMinSpeed){
				state.SweepFalse();
				state.isStillLandingSmall = true;
				state.isStillLanding = true;
				animator.Play("PlayerLandingStillSmall");
				stillLandTimer = stillLandLittleTime;
			} else{
				if(Mathf.Abs(rigidbody.velocity.x) > slideStopSpeedTarget){
					state.isRunning = true;
				} else{
					state.isStanding = true;
					animator.Play("PlayerIdle");
				}
			}
			Land();
			movementLocked = false;
		}
		
		if(state.isJumpBracing){
			rigidbody.AddForce(rigidbody.velocity * moveForce * -1.0f * slideForceMultiplier, ForceMode2D.Force);
		}
		
		if(state.isStillLanding){
			rigidbody.AddForce(rigidbody.velocity * moveForce * -2.0f * slideForceMultiplier, ForceMode2D.Force);
		}
	}
	
	void HandleJumpBracing(){
		if(state.isJumpBracing){
			movementLocked = true;
			jumpBraceCounter += Time.deltaTime;
			
			if(!jumpPressed){
				// check if player held down the jump button long enough to meet minimum length to execute jump
				float jumpForceMultiplier = Mathf.Clamp(jumpBraceCounter, 0.0f, jumpBraceTime) / jumpBraceTime;
				if(jumpForceMultiplier > minimumJumpBraceRatio){
					animator.Play("PlayerJumpingStill");
					state.isJumpBracing = false;
					state.isStillJumpLaunching = true;
					state.isJumping = true;
					state.isStillJumping = true;
					movementLocked = true;
					jumpLaunchTimer = jumpLaunchTime;
					aimAngle = Mathf.Atan2(vertical, horizontal);
				} else{
					state.isJumpBracing = false;
					state.isStanding = true;
					animator.Play("PlayerIdle");
					movementLocked = false;
				}
				
				
			} else if(!player.physics.isGrounded){
				state.SweepFalse();
				state.isStillJumping = true;
				state.isJumping = true;
				animator.Play("PlayerSoaringStill");
			}
		} else if(state.isStillJumpLaunching){
			jumpLaunchTimer -= Time.deltaTime;
			if(jumpLaunchTimer <= 0){
				state.SweepFalse();
				state.isJumping = true;
				movementLocked = true;
				state.isStillJumping = true;
				stillJumped = true;
			}
		}
	}
	
	void HandleMoveInput(){
		Vector2 force = new Vector2(horizontal, 0);
		if(player.physics.isGrounded && !state.isJumping && !state.isJumpBracing && !state.isStillJumping && !state.isStillLanding && !state.isCornerMantling){
			if(state.isSlideStopping){
				// stop slide stopping if low enough speed met to exit sliding
				if(Mathf.Abs(rigidbody.velocity.x) < slideStopSpeedTarget){
					state.isSlideStopping = false;
					state.isStanding = true;
					animator.Play("PlayerIdle");
				// switch to slide turning if player moves in opposite direction of current slide stop
				} else if(Mathf.Sign(horizontal) != Mathf.Sign(rigidbody.velocity.x) && horizontal != 0.0f){
					animator.Play("PlayerSlideTurning");
					state.isSlideTurning = true;
					state.isSlideStopping = false;
				} else if(Mathf.Sign(horizontal) == Mathf.Sign(rigidbody.velocity.x) && horizontal != 0.0f){
					state.isSlideStopping = false;
					state.isRunning = true;
					animator.Play("PlayerRunning");
				}
			} else if(state.isSlideTurning){
				// stop slide turning if low enough speed met to exit sliding
				if(Mathf.Abs(rigidbody.velocity.x) < slideStopSpeedTarget){
					state.isSlideTurning = false;
					state.isStanding = true;
					animator.Play("PlayerIdle");
				}
				
				// slide is opposite direction as typical, so do the extra flipping step here
				VelocityRotationHelper();
			} else if(state.isStanding){
				// check if player is running
				if(Mathf.Abs(horizontal) > 0.0f){
					state.isStanding = false;
					state.isRunning = true;
					animator.Play("PlayerRunning");
					
					// check if the joystick threshold to fully run ahs been crossed
					state.isFullRunning = (Mathf.Abs(horizontal) > walkJoystickThreshold);
				}
			} else if(state.isRunning && !movementLocked){
				// check if player stops running (but is not changing directions)
				if(horizontal == 0.0f){
					animator.Play("PlayerSlideStopping");
					state.isSlideStopping = true;
					state.isRunning = false;
				// check if player is suddenly changing directions
				} else if(Mathf.Sign(horizontal) != Mathf.Sign(rigidbody.velocity.x) && Mathf.Abs(rigidbody.velocity.x) > slideStopSpeedTarget){
					animator.Play("PlayerSlideTurning");
					state.isSlideTurning = true;
					state.isRunning = false;
				// add running force to continue movement
				} else{
					animator.Play("PlayerRunning");
					state.isRunning = true;
					
					// check if the joystick threshold to fully run ahs been crossed
					state.isFullRunning = (Mathf.Abs(horizontal) > walkJoystickThreshold);
				}
			} else if(state.isWallSplatStumbling){
				rigidbody.velocity = new Vector2(wallSplatStumbleSpeed * wallSplatSide * -1.0f, rigidbody.velocity.y);
			} else if(state.isWallSplatting){
				rigidbody.velocity = new Vector2(0.0f, 0.0f);
			} else{
				state.isStanding = true;
				animator.Play("PlayerIdle");
			}
			
			// sync isRunning and isFullRunning if needed
			if(!state.isRunning){
				state.isFullRunning = false;
			}
			
			// prevent player from exceeding maximum run speed
			if(Mathf.Abs(rigidbody.velocity.x) > maxRunSpeed){
				rigidbody.velocity = new Vector2(maxRunSpeed * Mathf.Sign(rigidbody.velocity.x), rigidbody.velocity.y);
			}
		}
		
		if(!movementLocked){
			if(horizontal > 0)
			{
				 gameObject.transform.parent.transform.eulerAngles = new Vector2(0,0);
				 state.direction = 1;
			}
			if(horizontal < 0)
			{
				gameObject.transform.parent.transform.eulerAngles = new Vector2(0,180);
				state.direction = -1;
			}
		}
    }
	
	void HandleMovePhysics(){
		Vector2 force = new Vector2(horizontal, 0);
		if(player.physics.isGrounded && !state.isJumping && !state.isJumpBracing && !state.isStillJumping && !state.isStillLanding){
			if(state.isSlideStopping){
				// apply resistive horizontal force
				rigidbody.AddForce(rigidbody.velocity * moveForce * -1.0f * slideForceMultiplier, ForceMode2D.Force);
			} else if(state.isSlideTurning){
				// apply resistive force
				rigidbody.AddForce(rigidbody.velocity * moveForce * -1.0f * slideForceMultiplier, ForceMode2D.Force);
			} else if(state.isStanding){
				// apply resistive force to prevent some residual sliding after ending a slide stop/turn
				rigidbody.AddForce(rigidbody.velocity * moveForce * -1.0f, ForceMode2D.Force);
			} else if(state.isRunning){
				rigidbody.AddForce(force * moveForce, ForceMode2D.Force);
			}
		}
	}
	
	void HandleWallColliding(){
		if(player.physics.isWalled){
			if(Mathf.Abs(player.physics.frontCollisionSpeed.x) > 0.0f && !player.physics.isGrounded && !state.isWallColliding){
				state.SweepFalse();
				state.isWallColliding = true;
				wallCollisionVelocity = new Vector2(player.physics.frontCollisionSpeed.x, player.physics.frontCollisionSpeed.y);
				//Debug.Log(wallCollisionVelocity);
				if(!state.isBracing && Mathf.Abs(rigidbody.velocity.x) > wallAirSplatMinSpeed){
					state.isAirWallSplatting = true;
					// TODO ADD BEHAVIOR FOR SPLATTING WALL IN THE AIR
				} else{
					state.isWallBracing = true;
					animator.Play("PlayerWallBracing");
					state.isBracing = false;
					wallBraceTimer = wallBraceTime;
					
				}
			}
		}
	}
	
	void HandleWallBracing(){
		if(state.isWallBracing){
			wallBraceTimer -= Time.deltaTime;
			if(wallBraceTimer <= 0){
				state.isWallBracing = false; 
				state.isWallPushing = true;
				animator.Play("PlayerWallPushing");
				wallPushTimer = wallPushTime;
			} else if(jumpJustPressed){
				wallLaunched = true; 
				animator.Play("PlayerWallLaunching");
				state.isWallLaunching = true; 
				state.isJumping = true; 
				state.isRunJumping = true;
				state.isWallBracing = false;
			} else if(!player.physics.isWalled){
				state.SweepFalse();
				state.isStillJumping = true;
				state.isJumping = true;
				animator.Play("PlayerSoaringStill");
			}
		}
	}
	
	void HandleWallLaunching(){
		if(state.isWallLaunching){
			wallLaunchTimer -= Time.deltaTime;
			if(wallLaunchTimer <= 0.0f){
				state.isWallLaunching = false; 
				state.isJumping = true; 
				state.isRunJumping = true;
				if(player.physics.wallSide == 1){
					gameObject.transform.parent.transform.eulerAngles = new Vector2(0,180);
				} else{
					gameObject.transform.parent.transform.eulerAngles = new Vector2(0,0);
				}
			}
		}
	}
	
	void HandleWallPushing(){
		if(state.isWallPushing){
			wallPushTimer -= Time.deltaTime;
			if(wallPushTimer <= 0.0f){
				state.isWallPushing = false;
				state.isJumping = true;
				state.isStillJumping = true;
				wallPushed = true;
			}
		}
	}
	
	void HandleWallCollidingPhysics(){		
		if(state.isWallBracing){
			if(rigidbody.velocity.y > 0){
				rigidbody.AddForce(rigidbody.velocity * -1.0f * wallSlideUpwardsCoefficient, ForceMode2D.Force);
			} else{
				rigidbody.AddForce(rigidbody.velocity * -1.0f * wallSlideDownwardsCoefficient, ForceMode2D.Force);
			}
		} else if(state.isWallLaunching || state.isWallPushing){
			rigidbody.AddForce(rigidbody.velocity * -1.0f * wallLaunchCoefficient, ForceMode2D.Force);
		}
		
		if(wallPushed){
			rigidbody.velocity = new Vector2(wallCollisionVelocity.x * wallPushHorizontalRetention, rigidbody.velocity.y + wallPushBoost);
			wallPushed = false;
			state.isWallColliding = false;
		} else if(wallLaunched){
			float jumpDir = Mathf.Sign(wallCollisionVelocity.x);
			rigidbody.velocity = new Vector2((Mathf.Clamp(Mathf.Abs(wallCollisionVelocity.x) * wallLaunchHorizontalRetention, wallLaunchMinimumHorizontal, 10000.0f)) * jumpDir, Mathf.Clamp((wallCollisionVelocity.y * -1.0f) + wallLaunchBoost, -1000f, wallLaunchMaxVerticalSpeed));
			wallLaunched = false;
			state.isWallColliding = false;
		}
	}
	
	void HandleWallSplatting(){
		if(player.physics.isWalled){
			if(Mathf.Abs(player.physics.frontCollisionSpeed.x) > wallSplatMinSpeed && player.physics.isGrounded && !state.isJumping){
				state.isRunning = false;
				state.isSlideStopping = false;
				state.isSlideTurning = false;
				state.isStanding = false;
				state.isWallSplatting = true;
				animator.Play("PlayerWallSplatting");
				movementLocked = true;
				wallSplatStickTimer = wallSplatStickTime;
				wallSplatSide = (int)Mathf.Sign(player.physics.frontCollisionSpeed.x) * -1;
				
				// make sure player is facing the wall they are splatting into
				if(wallSplatSide == 1){
					gameObject.transform.parent.transform.eulerAngles = new Vector2(0,0);
				} else{
					gameObject.transform.parent.transform.eulerAngles = new Vector2(0,180);
				}
			}
		}
	}
	
	void HandleWallSplatSticking(){
		if(state.isWallSplatting){
			wallSplatStickTimer -= Time.deltaTime;
			if(wallSplatStickTimer <= 0){
				state.isWallSplatting = false;
				state.isWallSplatStumbling = true;
				animator.Play("PlayerWallSplatStumbling");
				wallSplatStumbleTimer = wallSplatStumbleTime;
				rigidbody.velocity = new Vector2(wallSplatStumbleSpeed * wallSplatSide * -1.0f, rigidbody.velocity.y);
			}
		}
	}
	
	void HandleWallSplatStumbling(){
		if(state.isWallSplatStumbling){
			wallSplatStumbleTimer -= Time.deltaTime;
			if(wallSplatStumbleTimer <= 0){
				state.isWallSplatStumbling = false;
				state.isStanding = true;
				animator.Play("PlayerIdle");
				rigidbody.velocity = new Vector2(0.0f, rigidbody.velocity.y);
				movementLocked = false;
			}
		}
	}
	
	void HandleCornerGrabbing(){
		if(state.isBracing && !state.isCornerGrabbing && !state.isCornerClimbing && !state.isCornerMantling){
			if(cornerHandler.corner != null){
				state.SweepFalse();
				state.isCornerGrabbing = true;
				animator.Play("PlayerCornerGrabbing");
				gravityCache = rigidbody.gravityScale;
				rigidbody.gravityScale = 0f;
				rigidbody.velocity = new Vector2(0f,0f);
				if(transform.position.x > cornerHandler.corner.transform.position.x){
					cornerDir = 1;
				} else{
					cornerDir = -1;
				}
				
				transform.parent.position = new Vector3(cornerHandler.corner.position.x + (cornerHandler.cornerOffsetX * cornerDir), cornerHandler.corner.position.y - cornerHandler.cornerOffsetY, 0);
			} else if(cornerHandler.mantleCorner != null){
				state.SweepFalse();
				state.isCornerMantling = true;
				animator.Play("PlayerCornerMantling");
				movementLocked = true;
				gravityCache = rigidbody.gravityScale;
				rigidbody.gravityScale = 0f;
				rigidbody.velocity = new Vector2(0f,0f);
				if(transform.position.x > cornerHandler.mantleCorner.transform.position.x){
					cornerDir = 1;
				} else{
					cornerDir = -1;
				}
				transform.parent.position = new Vector3(cornerHandler.mantleCorner.position.x + (cornerHandler.mantleClimbOffsetX * cornerDir), cornerHandler.mantleCorner.position.y - cornerHandler.mantleClimbOffsetY, 0);
				cornerClimbTimer = cornerMantleTime;
			}
		} else if(state.isCornerGrabbing){
			if(vertical <= -(cornerClimbVertJoystickThreshold) || braceJustPressed){
				if(!climbJoystickTooFar){
					state.isBracing = false;
					state.isCornerGrabbing = false;
					state.isJumping = true;
					state.isStillJumping = true; 
					animator.Play("PlayerSoaringStill");
					rigidbody.gravityScale = gravityCache;
					movementLocked = false;
				}
			} else if(vertical >= cornerClimbVertJoystickThreshold){
				if(!climbJoystickTooFar){
					state.isBracing = false;
					state.isCornerGrabbing = false;
					state.isCornerClimbing = true;
					animator.Play("PlayerCornerClimbing");
					transform.parent.position = new Vector3(cornerHandler.corner.position.x + (cornerHandler.cornerClimbOffsetX * cornerDir), cornerHandler.corner.position.y - cornerHandler.cornerClimbOffsetY, 0);
					cornerClimbTimer = cornerClimbTime;
				}
			} else{
				climbJoystickTooFar = false;
			}
		} else if(state.isCornerClimbing || state.isCornerMantling){
			cornerClimbTimer -= Time.deltaTime;
			if(cornerClimbTimer <= 0){
				cornerClimbEnding = true;
			}
		}
	}
	
	void HandleItemGrabbing(){
		if(itemGrabJustPressed){
			inventoryHandler.Pickup();
		}
	}
	
	// LITTLE HELPER METHODS
	public void VelocityRotationHelper(){
		if(rigidbody.velocity.x > 0){
			gameObject.transform.parent.transform.eulerAngles = new Vector2(0,180);
		} else{
			gameObject.transform.parent.transform.eulerAngles = new Vector2(0,0);
		}
	}
	
	public void InverseVelocityRotationHelper(){
		if(rigidbody.velocity.x > 0){
			gameObject.transform.parent.transform.eulerAngles = new Vector2(0,0);
		} else{
			gameObject.transform.parent.transform.eulerAngles = new Vector2(0,180);
		}
	}
	
	private void Land(){
		state.isJumping = false;
		state.isStillJumping = false;
		state.isRunJumping = false;
	}
	
	public void LockPlayer(){
		physControlLocked = true;
		movementLocked = true;
	}
	
	public void UnlockPlayer(){
		physControlLocked = false;
		movementLocked = false;
	}
	
	// controller button methods
    private void OnMove(InputValue value){
        Vector2 vector = value.Get<Vector2>();

        horizontal = vector.x;
        vertical = vector.y;
    }
	
	private void OnJump(){
        jumpPressed = !jumpPressed;
        jumpJustPressed = jumpPressed;
    }
	
	private void OnBrace(){
        bracePressed = !bracePressed;
        braceJustPressed = bracePressed;
    }
	
	private void OnInventory(){
		/*
		inventoryPressed = !inventoryPressed;
		inventoryJustPressed = inventoryPressed;
		*/
	}
	
	private void OnItemGrab(){
		/*
		itemGrabPressed = !itemGrabPressed;
		itemGrabJustPressed = itemGrabPressed;
		*/
	}
	
	private void OnMenuUp(){
		menuUpPressed = !menuUpPressed;
		menuUpJustPressed = menuUpPressed;
	}
	
	private void OnMenuDown(){
		menuDownPressed = !menuDownPressed;
		menuDownJustPressed = menuDownPressed;
	}
	
	private void OnMenuRight(){
		menuRightPressed = !menuRightPressed;
		menuRightJustPressed = menuRightPressed;
	}
	
	private void OnMenuLeft(){
		menuLeftPressed = !menuLeftPressed;
		menuLeftJustPressed = menuLeftPressed;
	}
	
	private void OnMenuPageRight(){
		menuPageRightPressed = !menuPageRightPressed;
		menuPageRightJustPressed = menuPageRightPressed;
	}
	
	private void OnMenuPageLeft(){
		menuPageLeftPressed = !menuPageLeftPressed;
		menuPageLeftJustPressed = menuPageLeftPressed;
	}
	
	private void OnInteract(){
		interactPressed = !interactPressed;
		interactJustPressed = interactPressed;
	}
}
