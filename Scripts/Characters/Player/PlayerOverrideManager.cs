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

    Transform followTarget;
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
                        break;
                    } else if (!walkingBegan){
                        if(Mathf.Abs(rb.velocity.x) <= walkToPointSpeed){
                            player.stateManager.ResetPlayer();
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

    public void WalkToPoint(float x){
        overrideActive = true;
        walkingBegan = false;
        action = "WalkToPoint";
        walkDirection = x > player.transform.position.x ? 1 : -1;
        walkTarget = x;
        playerInputManager.LockPlayer();
        if(Mathf.Abs(rb.velocity.x) > walkToPointSpeed){
            playerInputManager.OverridePlayer();
        } else{
            playerInputManager.EndOverridePlayer();
            animator.Play("PlayerWalking");
        }
    }
}
