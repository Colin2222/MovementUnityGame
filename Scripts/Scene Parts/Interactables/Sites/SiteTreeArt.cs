using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteTreeArt : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public AnimatorOverrideController animatorOverrideController;
    public int treeNum;
    public int siteIndex;
    public AnimationClip regularAnimation;
    public AnimationClip rustleAnimation;
    // Start is called before the first frame update
    void Start()
    {
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animatorOverrideController["tree_1_regular"] = regularAnimation;
        animatorOverrideController["tree_1_rustle"] = rustleAnimation;
        animator.runtimeAnimatorController = animatorOverrideController;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayRustleAnimation(){
        animator.Play("tree_rustle", 0, 0.0f);
    }
}
