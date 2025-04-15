using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOverrideManager : MonoBehaviour
{
    bool overrideActive = false;
    string action;
    public PlayerHub player;
    public Rigidbody2D rb;
    public PlayerStateManager playerStateManager;
    public PlayerInputManager playerInputManager;
    public Animator animator;

    public float walkToPointSpeed;
    int walkDirection;
    float walkTarget;
    bool walkingBegan = false;
    public bool doorTransitioning = false;

    Transform lookAtTarget;
    bool resetPlayerOnWalk;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        if(overrideActive){
            switch(action){
                case "WalkToPoint":
                    if(Mathf.Abs(player.transform.position.x - walkTarget) < 0.05){
                        overrideActive = false;
                        if(!doorTransitioning){
                            rb.velocity = new Vector2(0, rb.velocity.y);
                        }
                        playerInputManager.UnlockPlayer();
                        playerStateManager.ResetPlayer();
                        doorTransitioning = false;
                        if(lookAtTarget != null){
                            playerStateManager.SetDirection(lookAtTarget.position.x > player.transform.position.x ? 1 : -1);
                            lookAtTarget = null;
                        }
                        break;
                    } else if (!walkingBegan){
                        if(Mathf.Abs(rb.velocity.x) <= walkToPointSpeed){
                            if(resetPlayerOnWalk){
                                player.stateManager.ResetPlayer();
                            }
                            rb.velocity = new Vector2(walkDirection * walkToPointSpeed, rb.velocity.y);
                            walkingBegan = true;
                        }
                    } else{
                        animator.Play("PlayerWalking");
                        rb.velocity = new Vector2(walkDirection * walkToPointSpeed, rb.velocity.y);
                    }
                    break;
            }
        }
    }

    public void WalkToPoint(float x, Transform lookAtPoint = null, bool resetPlayerOnWalk = true){
        overrideActive = true;
        walkingBegan = false;
        action = "WalkToPoint";
        walkDirection = x > player.transform.position.x ? 1 : -1;
        walkTarget = x;
        playerStateManager.SetDirection(walkDirection);
        playerInputManager.LockPlayer();
        if(Mathf.Abs(rb.velocity.x) > walkToPointSpeed){
            playerInputManager.OverridePlayer();
        } else{
            playerInputManager.EndOverridePlayer();
            animator.Play("PlayerWalking");
        }

        if(lookAtPoint != null){
            lookAtTarget = lookAtPoint;
        }
        this.resetPlayerOnWalk = resetPlayerOnWalk;
    }
}
