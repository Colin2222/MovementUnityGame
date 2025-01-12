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
                        rb.velocity = new Vector2(0, rb.velocity.y);
                        playerInputManager.UnlockPlayer();
                        playerStateManager.ResetPlayer();
                        break;
                    } else{
                        rb.velocity = new Vector2(walkDirection * walkToPointSpeed, rb.velocity.y);
                    }
                    break;
            }
        }
    }

    public void WalkToPoint(float x){
        overrideActive = true;
        action = "WalkToPoint";
        walkDirection = x > player.transform.position.x ? 1 : -1;
        walkTarget = x;
        playerInputManager.LockPlayer();
        animator.Play("PlayerWalking");
    }
}
