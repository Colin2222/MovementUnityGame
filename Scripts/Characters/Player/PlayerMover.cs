using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
	public PlayerHub player;
	Rigidbody2D rigidbody;
	PlayerState state;
	Animator animator;
	
	bool movementLocked = false;
	
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
	public float jumpLaunchTime;
	float jumpLaunchTimer;
	public float minimumJumpBraceRatio;
	public float stillLandLittleTime;
	public float stillLandBigTime;
	float stillLandTimer;
	
	private float horizontal = 0;
    private float vertical = 0;
	
	
	
	// button pressing
    private bool jumpPressed = false;
    private bool jumpJustPressed = false;
	
    // Start is called before the first frame update
    void Start() 
    {
        rigidbody = player.rigidbody;
		state = player.state;
		animator = player.animator;
    }

    // Update is called once per frame
    void Update()
    {
		// animation updating
        animator.SetFloat("xSpeed", Mathf.Abs(rigidbody.velocity.x));
        animator.SetFloat("ySpeed",rigidbody.velocity.y);
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
		
		HandleWallSplatSticking();
		HandleWallSplatStumbling();
		HandleJumping();
		HandleJumpBracing();
		HandleStillLanding();
		
		jumpJustPressed = false;
    }
	
	void FixedUpdate(){
		HandleFalling();
		HandleJumpingPhysics();
		HandleMove();
		HandleWallSplatting();
		CheckGroundedness();
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
	
	void HandleFalling(){
		
	}
	
	void HandleStillLanding(){
		if(state.isStillLandingSmall || state.isStillLandingBig){
			Debug.Log("LANDING");
			stillLandTimer -= Time.deltaTime;
			if(stillLandTimer <= 0){
				state.isStillLanding = false;
				state.isStillLandingBig = false;
				state.isStillLandingSmall = false;
				state.isStanding = true;
			}
		}
	}
	
	void HandleJumping(){
        // basic jump off ground (including coyote time)
        if((jumpJustPressed || timeSincePressed < jumpForgivenessTime) &&
        (player.physics.isGrounded /* || (timeSinceGrounded < coyoteTime) */ && !state.isJumping)){
			// handle running jumps
			if(!state.isSlideTurning && !state.isSlideStopping && Mathf.Abs(rigidbody.velocity.x) > runningJumpSpeed){ 
				jumped = true;
				state.SweepFalse();
				state.isJumping = true;
				state.isRunJumping = true;
				jumpTimeCounter = jumpTime;
				rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
			} else if(Mathf.Abs(rigidbody.velocity.x) <= runningJumpSpeed){
				state.SweepFalse();
				state.isJumpBracing = true;
				jumpBraceCounter = 0.0f;
			}
        }
		/*
		// check for extra jumping
		if(state.isJumping){
			if(jumpPressed){
				if(jumpTimeCounter > 0){
					state.isExtraJumping = true;
					jumpTimeCounter -= Time.deltaTime;
				} else{
					state.isExtraJumping = false;
				}
			} else{
				state.isExtraJumping = false;
			}
		}
		*/
	}
	
	void HandleJumpingPhysics(){
		if(jumped){
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
			rigidbody.AddForce(new Vector2(0,jumpForce), ForceMode2D.Impulse);
			jumped = false;
		} else if(stillJumped){
			rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
			float jumpForceMultiplier = Mathf.Clamp(jumpBraceCounter, 0.0f, jumpBraceTime) / jumpBraceTime;
			rigidbody.AddForce(new Vector2(0,jumpForce * jumpForceMultiplier), ForceMode2D.Impulse);
			stillJumped = false;
        } else if(state.isFalling && player.physics.isGrounded){
			if(state.isStillJumping){
				state.isStillLandingSmall = true;
				state.isStillLanding = true;
				stillLandTimer = stillLandLittleTime;
			} else{
				state.isRunning = true;
			}
			state.isJumping = false;
			state.isFalling = false;
			state.isExtraJumping = false;
			state.isRunJumping = false;
			state.isStillJumping = false;
		}
		
		if(state.isJumping && !player.physics.isGrounded){
			state.isFalling = true;
		}
		
		
		if(state.isExtraJumping){
			rigidbody.AddForce(new Vector2(0,jumpForce), ForceMode2D.Force);
		}
		
		if(state.isJumpBracing){
			rigidbody.AddForce(rigidbody.velocity * moveForce * -1.0f * slideForceMultiplier, ForceMode2D.Force);
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
					jumpLaunchTimer = jumpLaunchTime;
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
				state.isStillJumping = true;
				movementLocked = false;
				stillJumped = true;
			}
		}
	}
	
	void HandleMove()
    {
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
			} else if(state.isRunning){
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
		
		if(!movementLocked && !state.isJumping){
			if(horizontal > 0)
			{
				 gameObject.transform.parent.transform.eulerAngles = new Vector2(0,0);
			}
			if(horizontal < 0)
			{
				gameObject.transform.parent.transform.eulerAngles = new Vector2(0,180);
			}
		}
    }
	
	void HandleWallSplatting(){
		if(player.physics.isWalled){
			if(Mathf.Abs(player.physics.frontCollisionSpeed) > wallSplatMinSpeed && player.physics.isGrounded){
				state.isRunning = false;
				state.isSlideStopping = false;
				state.isSlideTurning = false;
				state.isStanding = false;
				state.isWallSplatting = true;
				movementLocked = true;
				wallSplatStickTimer = wallSplatStickTime;
				wallSplatSide = (int)Mathf.Sign(player.physics.frontCollisionSpeed) * -1;
				
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
	
	// LITTLE HELPER METHODS
	private void VelocityRotationHelper(){
		if(rigidbody.velocity.x > 0){
			gameObject.transform.parent.transform.eulerAngles = new Vector2(0,180);
		} else{
			gameObject.transform.parent.transform.eulerAngles = new Vector2(0,0);
		}
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
}
