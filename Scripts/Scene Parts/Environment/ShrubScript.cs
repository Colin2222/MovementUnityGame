using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrubScript : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public AnimatorOverrideController animatorOverrideController;
    public AnimationClip regularAnimation;
    public AnimationClip rustleRightAnimation;
    public AnimationClip rustleLeftAnimation;
    int direction; // -1 = left, 0 = center, 1 = right

    void Start()
    {
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animatorOverrideController["shrub_1_regular"] = regularAnimation;
        animatorOverrideController["shrub_1_rustle_right"] = rustleRightAnimation;
        animatorOverrideController["shrub_1_rustle_left"] = rustleLeftAnimation;
        animator.runtimeAnimatorController = animatorOverrideController;
        animator.Play("shrub_regular", 0, Random.Range(0.0f, 1.0f));
    }

    void OnTriggerEnter2D(Collider2D collision){
        Vector3 velocity = collision.attachedRigidbody.velocity;
        if (velocity.x > 3.0f)
        {
            animator.Play("shrub_rustle_right", 0, Random.Range(0.0f, 1.0f));
        } else if (velocity.x < -3.0f)
        {
            animator.Play("shrub_rustle_left", 0, Random.Range(0.0f, 1.0f));
        }
    }
}
