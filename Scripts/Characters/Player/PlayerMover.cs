using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
	public PlayerHub player;
	public CornerHandler cornerHandler;
	public PlayerInventoryHandler inventoryHandler;
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
	float cornerClimbTimer;
	bool cornerClimbEnding = false;
	
	private float horizontal = 0;
    private float vertical = 0;
	
	
	
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
		// animation updating
		if(!cornerClimbEnding){
			animator.SetFloat("xSpeed", Mathf.Abs(rigidbody.velocity.x));
			animator.SetFloat("ySpeed",rigidbody.velocity.y);
			animator.SetFloat("timeSinceGrounded", timeSinceGrounded);
			animator.SetBool("isStanding",state.isStanding);
			animator.SetBool("isRunning",state.isRunning);
			animator.SetBool("isFullRunning",state.isFullRunning);
			animator.SetBool("isJumping",state.isJumping);
			animator.SetBool("isSlideStopping",state.isSlideStopping);
			animator.SetBool("isSlideTurning", state.isSlideTurning);
			animator.SetBool("isWallSplatting", state.isWallSplatting);
			animator.SetBool("isWallSplatStumbling", state.isWallSplatStumbling);
			animator.SetBool("isRunJumping", state.isRunJumping);
			animator.SetBool("isJumpBracing", state.isJumpBracing);
			animator.SetBool("isStillJumping", state.isStillJumping);
			animator.SetBool("isStillJumpLaunching", state.isStillJumpLaunching);
			animator.SetBool("isStillLandingBig", state.isStillLandingBig);
			animator.SetBool("isStillLandingSmall", state.isStillLandingSmall);
			animator.SetBool("isWallBracing", state.isWallBracing);
			animator.SetBool("isWallPushing", state.isWallPushing);
			animator.SetBool("isWallLaunching", state.isWallLaunching);
			animator.SetBool("isAirWallSplatting", state.isAirWallSplatting);
			animator.SetBool("isCornerGrabbing", state.isCornerGrabbing);
			animator.SetBool("isCornerClimbing", state.isCornerClimbing);
			animator.SetBool("isInInventory", state.isInInventory);
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
		HandleInventory();
		HandleItemGrabbing();
		
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
    }
	
	void FixedUpdate(){
		if(!state.isCornerGrabbing && !state.isCornerClimbing){
			HandleJumpingPhysics();
			HandleMove();
			HandleWallSplatting();
			HandleWallColliding();
			HandleWallCollidingPhysics();
			CheckGroundedness();
		} else{
			if(cornerClimbEnding){
				cornerClimbEnding = false;
				if(cornerHandler.corner == null){
					transform.parent.position = new Vector3(cornerHandler.lastCorner.position.x + (cornerHandler.cornerEndClimbOffsetX * cornerDir * -1), cornerHandler.lastCorner.position.y + cornerHandler.cornerEndClimbOffsetY, 0);
				} else{
					transform.parent.position = new Vector3(cornerHandler.corner.position.x + (cornerHandler.cornerEndClimbOffsetX * cornerDir * -1), cornerHandler.corner.position.y + cornerHandler.cornerEndClimbOffsetY, 0);
				}
				rigidbody.gravityScale = gravityCache;
				movementLocked = false;
				state.isCornerClimbing = false;
				state.isStanding = true;
				state.direction = cornerDir;
			}
		}
	}
	
	void CheckGroundedness(){
		// update coyote time and jump forgiveness time
		if(player.physics.isGrounded){
            timeSinceGrounded = 0.0f;
        } else{
            timeSinceGrounded += Time.fixedDeltaTime;
		}
		if(timeSincePressed < jumpForgivenessTime){
            timeSincePressed += Time.fixedDeltaTime;
        }
	}
	
	void HandleInventory(){
		if(inventoryJustPressed){
			state.SweepFalse();
			state.isInInventory = inventoryHandler.ToggleInventory();
			movementLocked = state.isInInventory;
			
			if(!state.isInInventory){
				state.isStanding = true;
				animator.Play("PlayerBagExiting");
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
				state.isStillLandingSmall = true;
				state.isStillLanding = true;
				stillLandTimer = stillLandLittleTime;
			} else{
				state.isRunning = true;
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
				}
				
				
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
	
	void HandleMove(){
		Vector2 force = new Vector2(horizontal, 0);
		if(player.physics.isGrounded && !state.isJumping && !state.isJumpBracing && !state.isStillJumping && !state.isStillLanding){
			if(state.isSlideStopping){
				// apply resistive horizontal force
				rigidbody.AddForce(rigidbody.velocity * moveForce * -1.0f * slideForceMultiplier, ForceMode2D.Force);
				
				// stop slide stopping if low enough speed met to exit sliding
				if(Mathf.Abs(rigidbody.velocity.x) < slideStopSpeedTarget){
					state.isSlideStopping = false;
					state.isStanding = true;
				// switch to slide turning if player moves in opposite direction of current slide stop
				} else if(Mathf.Sign(horizontal) != Mathf.Sign(rigidbody.velocity.x) && horizontal != 0.0f){
					state.isSlideTurning = true;
					state.isSlideStopping = false;
				} else if(Mathf.Sign(horizontal) == Mathf.Sign(rigidbody.velocity.x) && horizontal != 0.0f){
					state.isSlideStopping = false;
					state.isRunning = true;
				}
			} else if(state.isSlideTurning){
				// apply resistive force
				rigidbody.AddForce(rigidbody.velocity * moveForce * -1.0f * slideForceMultiplier, ForceMode2D.Force);
				
				// stop slide turning if low enough speed met to exit sliding
				if(Mathf.Abs(rigidbody.velocity.x) < slideStopSpeedTarget){
					state.isSlideTurning = false;
					state.isStanding = true;
				}
				
				// slide is opposite direction as typical, so do the extra flipping step here
				VelocityRotationHelper();
			} else if(state.isStanding){
				// check if player is running
				if(Mathf.Abs(horizontal) > 0.0f){
					state.isStanding = false;
					state.isRunning = true;
					
					// check if the joystick threshold to fully run ahs been crossed
					state.isFullRunning = (Mathf.Abs(horizontal) > walkJoystickThreshold);
				// apply resistive force to prevent some residual sliding after ending a slide stop/turn
				} else {
					rigidbody.AddForce(rigidbody.velocity * moveForce * -1.0f, ForceMode2D.Force);
				}
			} else if(state.isRunning && !movementLocked){
				// check if player stops running (but is not changing directions)
				if(horizontal == 0.0f){
					state.isSlideStopping = true;
					state.isRunning = false;
				// check if player is suddenly changing directions
				} else if(Mathf.Sign(horizontal) != Mathf.Sign(rigidbody.velocity.x) && Mathf.Abs(rigidbody.velocity.x) > slideStopSpeedTarget){
					state.isSlideTurning = true;
					state.isRunning = false;
				// add running force to continue movement
				} else{
					state.isRunning = true;
					rigidbody.AddForce(force * moveForce, ForceMode2D.Force);
					
					// check if the joystick threshold to fully run ahs been crossed
					state.isFullRunning = (Mathf.Abs(horizontal) > walkJoystickThreshold);
				}
			} else if(state.isWallSplatStumbling){
				rigidbody.velocity = new Vector2(wallSplatStumbleSpeed * wallSplatSide * -1.0f, rigidbody.velocity.y);
			} else if(state.isWallSplatting){
				rigidbody.velocity = new Vector2(0.0f, 0.0f);
			} else{
				state.isStanding = true;
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
	
	void HandleWallSplatting(){
		if(player.physics.isWalled){
			if(Mathf.Abs(player.physics.frontCollisionSpeed.x) > wallSplatMinSpeed && player.physics.isGrounded && !state.isJumping){
				state.isRunning = false;
				state.isSlideStopping = false;
				state.isSlideTurning = false;
				state.isStanding = false;
				state.isWallSplatting = true;
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
				wallPushTimer = wallPushTime;
			} else if(jumpJustPressed){
				wallLaunched = true; 
				state.isWallLaunching = false; 
				state.isJumping = true; 
				state.isRunJumping = true;
				state.isWallBracing = false;
				/*
				state.isWallBracing = false;
				state.isWallLaunching = true;
				wallLaunchTimer = wallLaunchTime;
				*/
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
				wallLaunched = true;
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
			InverseVelocityRotationHelper();
		} else if(wallLaunched){
			float jumpDir = Mathf.Sign(wallCollisionVelocity.x);
			rigidbody.velocity = new Vector2((Mathf.Clamp(Mathf.Abs(wallCollisionVelocity.x) * wallLaunchHorizontalRetention, wallLaunchMinimumHorizontal, 10000.0f)) * jumpDir, Mathf.Clamp((wallCollisionVelocity.y * -1.0f) + wallLaunchBoost, -1000f, wallLaunchMaxVerticalSpeed));
			wallLaunched = false;
			state.isWallColliding = false;
			InverseVelocityRotationHelper();
		}
	}
	
	void HandleWallSplatSticking(){
		if(state.isWallSplatting){
			wallSplatStickTimer -= Time.deltaTime;
			if(wallSplatStickTimer <= 0){
				state.isWallSplatting = false;
				state.isWallSplatStumbling = true;
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
				rigidbody.velocity = new Vector2(0.0f, rigidbody.velocity.y);
				movementLocked = false;
			}
		}
	}
	
	void HandleCornerGrabbing(){
		if(state.isBracing && !state.isCornerGrabbing && !state.isCornerClimbing && cornerHandler.corner != null){
			state.SweepFalse();
			state.isCornerGrabbing = true;
			gravityCache = rigidbody.gravityScale;
			rigidbody.gravityScale = 0f;
			rigidbody.velocity = new Vector2(0f,0f);
			if(transform.position.x > cornerHandler.corner.transform.position.x){
				cornerDir = 1;
			} else{
				cornerDir = -1;
			}
			
			transform.parent.position = new Vector3(cornerHandler.corner.position.x + (cornerHandler.cornerOffsetX * cornerDir), cornerHandler.corner.position.y - cornerHandler.cornerOffsetY, 0);
		} else if(braceJustPressed && state.isCornerGrabbing){
			if(vertical <= 0){
				state.isBracing = false;
				state.isCornerGrabbing = false;
				state.isJumping = true;
				state.isStillJumping = true; 
				rigidbody.gravityScale = gravityCache;
				movementLocked = false;
			} else{
				state.isBracing = false;
				state.isCornerGrabbing = false;
				state.isCornerClimbing = true;
				transform.parent.position = new Vector3(cornerHandler.corner.position.x + (cornerHandler.cornerClimbOffsetX * cornerDir), cornerHandler.corner.position.y - cornerHandler.cornerClimbOffsetY, 0);
				cornerClimbTimer = cornerClimbTime;
			}
		} else if(state.isCornerClimbing){
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
		inventoryPressed = !inventoryPressed;
		inventoryJustPressed = inventoryPressed;
	}
	
	private void OnItemGrab(){
		itemGrabPressed = !itemGrabPressed;
		itemGrabJustPressed = itemGrabPressed;
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
}
