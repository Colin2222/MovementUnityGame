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
    public float blackoutTime;
    float totalCutsceneTime;
    float cutsceneTimer;
    bool inCutscene = false;
    SiteTreeArt treeArt;
    bool cutDown = false;

    public GameObject itemPrefab;
    int numLogs;
    public float logSpreadDistance;


    void Start(){
        GameObject[] treeArtArray = GameObject.FindGameObjectsWithTag("TreeArt");
        foreach(GameObject x in treeArtArray){
            SiteTreeArt art = x.GetComponent<SiteTreeArt>();
            if(art.siteIndex == id){
                treeArt = art;
                if(cutDown){
                    Destroy(treeArt.gameObject);
                    hasMenu = false;
                }
                break;
            }
        }
    }

    void Update(){
        if(!cutDown){
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
                    treeArt.PlayRustleAnimation();

                    if(numSwings >= 5){
                        inSwing = false;
                        SceneManager.Instance.cutsceneManager.StartFullscreenCutscene(cutsceneTime, blackoutTime, "tree_cutting_base", "bg_mountain_" + SessionManager.Instance.GetIntegerMarker("time_marker"));
                        cutsceneTimer = 0.0f;
                        inCutscene = true;
                        totalCutsceneTime = cutsceneTime + (blackoutTime * 2.0f);
                    }
                }
                return;
            }

            if(inCutscene){
                cutsceneTimer += Time.deltaTime;
                if(cutsceneTimer >= totalCutsceneTime){
                    inCutscene = false;
                    cutDown = true;
                    hasMenu = false;
                    PlayerHub.Instance.inputManager.UnlockPlayer();
                    PlayerHub.Instance.stateManager.ResetPlayer();
                } else if(treeArt != null && cutsceneTimer >= totalCutsceneTime / 2){
                    Destroy(treeArt.gameObject);
                    for(int i = 0; i < numLogs; i++){
                        float xOffset = Random.Range(-logSpreadDistance, logSpreadDistance);
                        Vector3 dropPosition = new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z);
                        WorldItem droppedItem = Instantiate(itemPrefab, dropPosition, Quaternion.identity).GetComponent<WorldItem>();
                        droppedItem.item_id = "wood_log";
                    }
                }
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
        if(cutDown){
            PlayerHub.Instance.inputManager.UnlockPlayer();
            PlayerHub.Instance.stateManager.ResetPlayer();
            return;
        }

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
        if(!cutDown && inPostSwing && !inCutscene){
            PlayerHub.Instance.animator.Play("PlayerAxeSwingRepeat", 0, 0.0f);
            ResetTimers();
            inSwing = true;
            inPostSwing = false;
            return;
        }
    }

    public override void LoadSite(SavedSite savedSite){
        numLogs = int.Parse(savedSite.additional_data["num_logs"]);
        cutDown = bool.Parse(savedSite.additional_data["cut_down"]);
    }

    public override SavedSite SaveSite(){
        SavedSite savedSite = new SavedSite();
        savedSite.name = "tree";
        savedSite.additional_data = new Dictionary<string, string>();
        savedSite.additional_data.Add("num_logs", numLogs.ToString());
        savedSite.additional_data.Add("cut_down", cutDown.ToString());
        return savedSite;
    }

    public override void ConstructSite(){
        
    }
}
