using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteTree : Site
{
    public Transform leftSwingPoint;
    public Transform rightSwingPoint;

    bool inSwing = false;
    bool inPostSwing = false;
    float swingTimer;
    public float swingTime;
    float postSwingTimer;
    public float postSwingTime;
    public float swingForgetTime;
    float swingForgetTimer;
    int numSwings = 0;

    public float cutsceneTime;
    float cutsceneTimer;
    bool inCutscene = false;

    void Update(){
        if(numSwings > 0){
            swingForgetTimer += Time.deltaTime;
            if(swingForgetTimer >= swingForgetTime){
                numSwings = 0;
            }
        }

        if(inPostSwing && !inCutscene){
            postSwingTimer += Time.deltaTime;
            if(postSwingTimer >= postSwingTime){
                inPostSwing = false;
                inSwing = false;
                PlayerHub.Instance.inputManager.UnlockPlayer();
                PlayerHub.Instance.stateManager.ResetPlayer();
            }
            return;
        }

        if(inSwing){
            swingTimer += Time.deltaTime;
            if(swingTimer >= swingTime){
                numSwings++;
                swingForgetTimer = 0.0f;
                ResetTimers();
                inSwing = false;
                inPostSwing = true;

                if(numSwings >= 5){
                    inSwing = false;
                    SceneManager.Instance.cutsceneManager.StartFullscreenCutscene(cutsceneTime, "tree_cutting_base");
                    cutsceneTimer = 0.0f;
                    inCutscene = true;
                }
            }
            return;
        }

        if(inCutscene){
            cutsceneTimer += Time.deltaTime;
            if(cutsceneTimer >= cutsceneTime){
                inCutscene = false;
                PlayerHub.Instance.inputManager.UnlockPlayer();
                PlayerHub.Instance.stateManager.ResetPlayer();
            }
        }
    }

    void ResetTimers(){
        swingTimer = 0.0f;
        postSwingTimer = 0.0f;
    }

    protected override void EnterRange(){
        
    }

    protected override void ExitRange(){
        
    }

    public override void Interact(){
        if(!(Mathf.Abs(PlayerHub.Instance.transform.position.x - rightSwingPoint.position.x) <= 0.05f || 
           Mathf.Abs(PlayerHub.Instance.transform.position.x - leftSwingPoint.position.x) <= 0.05f)){
            if(PlayerHub.Instance.transform.position.x > transform.position.x){
            PlayerHub.Instance.overrideManager.WalkToPoint(rightSwingPoint.position.x, transform, false);
            } else{
                PlayerHub.Instance.overrideManager.WalkToPoint(leftSwingPoint.position.x, transform, false);
            }
        } else{
            PlayerHub.Instance.animator.Play("PlayerAxeSwingRepeat");
            ResetTimers();
            inSwing = true;
            inPostSwing = false;
        }
    }

    public override void LeaveInteraction(){
        
    }

    public override void MenuUp(){
        
    }

    public override void MenuDown(){
        
    }

    public override void MenuLeft(){
        
    }

    public override void MenuRight(){
        
    }

    public override void MenuSelect(){
        
    }

    public override void MenuInteract()
    {
        if(inPostSwing){
            PlayerHub.Instance.animator.Play("PlayerAxeSwingRepeat", 0, 0.0f);
            ResetTimers();
            inSwing = true;
            inPostSwing = false;
            return;
        }
    }

    public override void LoadSite(SavedSite savedSite){
        
    }

    public override SavedSite SaveSite(){
        SavedSite savedSite = new SavedSite();
        savedSite.name = "tree";
        return savedSite;
    }

    public override void ConstructSite(){
        
    }
}
